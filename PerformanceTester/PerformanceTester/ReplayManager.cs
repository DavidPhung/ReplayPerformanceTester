using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceTester
{
    class ReplayManager
    {
        private string setupTrace;
        private string testTrace;
        private string snapshotName;
        private string backupFile;

        private string connectionString;
        private string processName;
        private string databaseName;
        private string replayMode;
        private string resetMethod;


        private GUIDataMonitor monitor;

        public List<long> RunTimeMillis { get; protected set; }
        public List<MemoryReader> MemReaders { get; protected set; }

        public ReplayManager(ProgramArguments args, GUIDataMonitor monitor)
        {
            connectionString = OdbcUtils.CreateConnectionString(args);
            RunTimeMillis = new List<long>();
            MemReaders = new List<MemoryReader>();
            snapshotName = args.Snapshot;
            processName = args.Process;
            databaseName = args.TestDatabase;
            setupTrace = args.SetupTraceFile;
            testTrace = args.TestTraceFile;
            replayMode = args.ReplayMode;
            resetMethod = args.ResetMethod;
            this.backupFile = args.BackupFile;
            this.monitor = monitor;
        }

        public void Run(int nbrRepeats)
        {
            ReplayUnit setupReplay = null;
            ReplayUnit testReplay = null;

            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                //Loading setup trace
                if (!setupTrace.Equals("--"))
                {
                    Console.Write("Loading setup trace " + setupTrace + " ... ");
                    string s = "select EventClass, TextData, EventSequence, StartTime, EndTime, SPID, DatabaseName " +
                                    "from sys.fn_trace_gettable(N'" + setupTrace + "', default) " +
                                    "where EventClass = 11 or EventClass = 13 or EventClass = 14 " +
                                    "or EventClass = 15 or EventClass = 17";
                    DataTable replayTable = OdbcUtils.ExecuteReader(conn, s);
                    TracePreprocessor.Preprocess(replayTable, databaseName);
                    setupReplay = new SingleConnectionReplayUnit(connectionString, databaseName);
                    DatabaseEventBuilder.Build(setupReplay, replayTable);
                    Console.WriteLine("completed");
                }
                Console.Write("Loading test trace " + testTrace + " ... ");
                string sql = "select EventClass, TextData, EventSequence, StartTime, EndTime, SPID, DatabaseName " +
                                "from sys.fn_trace_gettable(N'" + testTrace + "', default) " +
                                "where EventClass = 11 or EventClass = 13 or EventClass = 14 " +
                                "or EventClass = 15 or EventClass = 17";
                DataTable testTable = OdbcUtils.ExecuteReader(conn, sql);
                TracePreprocessor.Preprocess(testTable, databaseName);
                if (replayMode.Equals(ProgramArguments.REPLAY_MODE_MULTI_CONNECTION))
                {
                    testReplay = new ReplayUnit(connectionString, databaseName);
                }else
                {
                    testReplay = new SingleConnectionReplayUnit(connectionString, databaseName);
                }
                DatabaseEventBuilder.Build(testReplay, testTable);
                Console.WriteLine("completed");
            }

            RunTimeMillis.Clear();
            MemReaders.Clear();
            for (int i = 0; i < nbrRepeats; i++)
            {
                Console.WriteLine("----------");

                if (resetMethod.Equals(ProgramArguments.RESET_METHOD_SNAPSHOT))
                {
                    Console.Write("Restoring snapshot ... ");
                    SQLServerUtils.RestoreSnapshot(snapshotName, databaseName, connectionString);
                    Console.WriteLine("completed");
                }else if (resetMethod.Equals(ProgramArguments.RESET_METHOD_BACKUP))
                {
                    Console.Write("Restoring from backup ... ");
                    SQLServerUtils.RestoreFromBackup(backupFile, databaseName, connectionString);
                    Console.WriteLine("completed");
                }else
                {
                    Console.WriteLine("Invalid reset method");
                    break;
                }

                bool cancelled = false;
                if (!setupTrace.Equals("--"))
                {
                    Console.WriteLine("Running setup trace " + setupTrace + " (q to cancel)");
                    if (!RunReplayAsCancellableTask(setupReplay))
                    {
                        cancelled = true;
                        break;
                    }
                }

                if (cancelled)
                {
                    Console.WriteLine("Cancelled");
                    break;
                }

                testReplay.Reset();
                Console.WriteLine("Running test trace " + testTrace + " (q to cancel)");
                MemoryReader mr = new MemoryReader(processName);
                mr.StartMeasure();
                if (!RunReplayAsCancellableTask(testReplay))
                {
                    mr.EndMeasure();
                    Console.WriteLine("Cancelled");
                    break;
                }
                mr.EndMeasure();
                MemReaders.Add(mr);
                RunTimeMillis.Add(testReplay.RunTimeMillis);

                monitor.AddRunTimeMillis(testReplay.RunTimeMillis);
                monitor.AddAvgMem(mr.GetAverage());
            }
        }

        private bool RunReplayAsCancellableTask(ReplayUnit u)
        {
            bool completed = true;
            CancellationTokenSource cts = new CancellationTokenSource();
            Task task = Task.Run(() => u.Run(cts.Token));
            Task progressTask = Task.Run(() => DisplayProgress(u, cts.Token, task));

            //flush console input buffer
            while (Console.KeyAvailable) Console.ReadKey(true);

            while (!cts.IsCancellationRequested && !task.IsCompleted)
            {
                while (!Console.KeyAvailable && !task.IsCompleted)
                    Thread.Sleep(200);
                if (Console.KeyAvailable && Console.ReadKey(true).KeyChar == 'q')
                {
                    cts.Cancel();
                    completed = false;
                }
            }
            task.Wait();
            progressTask.Wait();

            return completed;
        }

        private void DisplayProgress(ReplayUnit u, CancellationToken token, Task t)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int total = u.NbrEvents;
            while (u.ExecutedEvents < u.NbrEvents)
            {
                if (token.IsCancellationRequested)
                {
                    t.Wait();
                    break;
                }
                Console.SetCursorPosition(left, top);
                int percentage = (int)(100f * u.ExecutedEvents / total);
                Console.Write(u.ExecutedEvents + " of " + total + " (" + percentage + "%)");
                Thread.Sleep(500);
            }
            Console.SetCursorPosition(left, top);
            int percentage1 = (int)(100f * u.ExecutedEvents / total);
            Console.WriteLine(u.ExecutedEvents + " of " + total + " (" + percentage1 + "%)");
        }
    }
}

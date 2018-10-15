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
        public List<string> SetupTraces { get; protected set; }
        public string TestTrace { get; set; }
        public string SnapshotName { get; protected set; }

        public List<long> RunTimeMillis { get; protected set; }
        public List<MemoryReader> MemReaders { get; protected set; }

        private ConnectionInfo connectionInfo;
        private string processName;
        private string databaseName;

        private GUIDataMonitor monitor;

        public ReplayManager(ConnectionInfo connectionInfo, string databaseUT, string snapshotName, string processName, GUIDataMonitor monitor)
        {
            SetupTraces = new List<string>();
            this.connectionInfo = connectionInfo;
            RunTimeMillis = new List<long>();
            MemReaders = new List<MemoryReader>();
            this.SnapshotName = snapshotName;
            this.processName = processName;
            this.monitor = monitor;
            this.databaseName = databaseUT;
        }

        public void Run(int nbrRepeats)
        {
            List<ReplayUnit> setupReplays = new List<ReplayUnit>();
            ReplayUnit testReplay = null;

            using (OdbcConnection conn = new OdbcConnection(OdbcUtils.CreateConnectionString(connectionInfo)))
            {
                conn.Open();
                foreach (var trace in SetupTraces)
                {
                    Console.Write("Loading setup trace " + trace + " ... ");
                    string s = "select EventClass, TextData, EventSequence, StartTime, EndTime, SPID, DatabaseName " +
                                    "from sys.fn_trace_gettable(N'" + trace + "', default) " +
                                    "where EventClass = 11 or EventClass = 13 or EventClass = 14 " +
                                    "or EventClass = 15 or EventClass = 17";
                    DataTable dt = OdbcUtils.ExecuteReader(conn, s);
                    TracePreprocessor.Preprocess(dt, databaseName);
                    ReplayUnit u = new SingleConnectionReplayUnit(connectionInfo, databaseName);
                    DatabaseEventBuilder.Build(u, dt);
                    setupReplays.Add(u);
                    Console.WriteLine("completed");
                }

                Console.Write("Loading test trace " + TestTrace + " ... ");
                string sql = "select EventClass, TextData, EventSequence, StartTime, EndTime, SPID, DatabaseName " +
                                "from sys.fn_trace_gettable(N'" + TestTrace + "', default) " +
                                "where EventClass = 11 or EventClass = 13 or EventClass = 14 " +
                                "or EventClass = 15 or EventClass = 17";
                DataTable testTable = OdbcUtils.ExecuteReader(conn, sql);
                TracePreprocessor.Preprocess(testTable, databaseName);
                testReplay = new SingleConnectionReplayUnit(connectionInfo, databaseName);
                DatabaseEventBuilder.Build(testReplay, testTable);
                Console.WriteLine("completed");
            }

            RunTimeMillis.Clear();
            MemReaders.Clear();
            for (int i = 0; i < nbrRepeats; i++)
            {
                Console.WriteLine("----------");

                Console.Write("Restoring snapshot ... ");
                SQLServerUtils.RestoreSnapshot(SnapshotName, databaseName, connectionInfo);
                Console.WriteLine("completed");

                bool cancelled = false;
                for (int j = 0; j < SetupTraces.Count; j++)
                {
                    Console.WriteLine("Running setup trace " + SetupTraces[j] + " (q to cancel)");
                    if (!RunReplayAsCancellableTask(setupReplays[j]))
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
                Console.WriteLine("Running test trace " + TestTrace + " (q to cancel)");
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

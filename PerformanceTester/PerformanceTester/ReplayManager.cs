﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo.Wmi;

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
        private int nbrWarmup;

        private GUIDataMonitor monitor;

        public List<long> RunTimeMillis { get; protected set; }
        public List<MemoryReader> MemReaders { get; protected set; }
        public MemoryReader StartMemReader;

        public ReplayManager(ProgramArguments args, GUIDataMonitor monitor)
        {
            connectionString = OdbcUtils.CreateConnectionString(args);
            snapshotName = args.Snapshot;
            processName = args.Process;
            databaseName = args.TestDatabase;
            setupTrace = args.SetupTraceFile;
            testTrace = args.TestTraceFile;
            replayMode = args.ReplayMode;
            resetMethod = args.ResetMethod;
            nbrWarmup = args.NbrWarmup;
            this.backupFile = args.BackupFile;
            this.monitor = monitor;
            RunTimeMillis = new List<long>();
            MemReaders = new List<MemoryReader>();
            StartMemReader = new MemoryReader(processName);
        }

        public void Run(int nbrRepeats)
        {
            ReplayUnit setupReplay = null;
            ReplayUnit testReplay = null;
            ReplayUnit warmupReplay = null;

            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                //Loading setup trace
                if (!setupTrace.Equals(""))
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
                    testReplay = new ReplayUnit(connectionString, databaseName, false);
                }
                else if (replayMode.Equals(ProgramArguments.REPLAY_MODE_MULTI_CONNECTION_WITH_DELAY))
                {
                    testReplay = new ReplayUnit(connectionString, databaseName, true);
                }
                else
                {
                    testReplay = new SingleConnectionReplayUnit(connectionString, databaseName);
                }
                DatabaseEventBuilder.Build(testReplay, testTable);
                Console.WriteLine("completed");

                warmupReplay = new SingleConnectionReplayUnit(connectionString, databaseName);
                DatabaseEventBuilder.Build(warmupReplay, testTable);
            }

            RunTimeMillis.Clear();
            MemReaders.Clear();

            Console.WriteLine("----------");
            Console.WriteLine("WARM UP");
            for (int i = 0; i < nbrWarmup; i++)
            {
                Console.WriteLine("----------");

                if (resetMethod.Equals(ProgramArguments.RESET_METHOD_SNAPSHOT))
                {
                    Console.Write("Restoring snapshot ... ");
                    SQLServerUtils.RestoreSnapshot(snapshotName, databaseName, connectionString);
                    Console.WriteLine("completed");
                }
                else if (resetMethod.Equals(ProgramArguments.RESET_METHOD_BACKUP))
                {
                    Console.Write("Restoring from backup ... ");
                    SQLServerUtils.RestoreFromBackup(backupFile, databaseName, connectionString);
                    Console.WriteLine("completed");
                }

                OdbcConnection.ReleaseObjectPool();

                bool cancelled = false;
                if (!setupTrace.Equals(""))
                {
                    setupReplay.Reset();
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

                warmupReplay.Reset();
                Console.WriteLine("Running test trace " + testTrace + " (q to cancel)");
                if (!RunReplayAsCancellableTask(warmupReplay))
                {
                    Console.WriteLine("Cancelled");
                    break;
                }
            }

            Console.WriteLine("----------");
            Console.WriteLine("ACTUAL TEST");
            for (int i = 0; i < nbrRepeats; i++)
            {
                Console.WriteLine("----------");

                if (resetMethod.Equals(ProgramArguments.RESET_METHOD_SNAPSHOT))
                {
                    Console.Write("Restoring snapshot ... ");
                    SQLServerUtils.RestoreSnapshot(snapshotName, databaseName, connectionString);
                    Console.WriteLine("completed");
                }
                else if (resetMethod.Equals(ProgramArguments.RESET_METHOD_BACKUP))
                {
                    Console.Write("Restoring from backup ... ");
                    SQLServerUtils.RestoreFromBackup(backupFile, databaseName, connectionString);
                    Console.WriteLine("completed");
                }

                OdbcConnection.ReleaseObjectPool();

                //Restart service
                //RestartService();

                bool cancelled = false;
                if (!setupTrace.Equals(""))
                {
                    setupReplay.Reset();
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

        private void RestartService()
        {
            //Declare and create an instance of the ManagedComputer   
            //object that represents the WMI Provider services.   
            ManagedComputer mc;
            mc = new ManagedComputer();
            //Iterate through each service registered with the WMI Provider.   

            foreach (Service svc in mc.Services)
            {
                Console.WriteLine(svc.Name);
            }
            //Reference the Microsoft SQL Server service.   
            Service Mysvc = mc.Services["MSSQLSERVER"];
            //Stop the service if it is running and report on the status  
            // continuously until it has stopped.   
            if (Mysvc.ServiceState == ServiceState.Running)
            {
                Mysvc.Stop();
                Console.WriteLine(string.Format("{0} service state is {1}", Mysvc.Name, Mysvc.ServiceState));
                while (!(string.Format("{0}", Mysvc.ServiceState) == "Stopped"))
                {
                    Console.WriteLine(string.Format("{0}", Mysvc.ServiceState));
                    Mysvc.Refresh();
                }
                Console.WriteLine(string.Format("{0} service state is {1}", Mysvc.Name, Mysvc.ServiceState));
                //Start the service and report on the status continuously   
                //until it has started.   
                Mysvc.Start();
                while (!(string.Format("{0}", Mysvc.ServiceState) == "Running"))
                {
                    Console.WriteLine(string.Format("{0}", Mysvc.ServiceState));
                    Mysvc.Refresh();
                }
                Console.WriteLine(string.Format("{0} service state is {1}", Mysvc.Name, Mysvc.ServiceState));
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("SQL Server service is not running.");
                Console.ReadLine();
            }
        }
    }
}

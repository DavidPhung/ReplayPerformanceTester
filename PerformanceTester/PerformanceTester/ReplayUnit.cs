using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace PerformanceTester
{
    public class ReplayUnit : DatabaseEventExecutionContext
    {
        public IList<DatabaseEvent> Events { get; }
        public string DatabaseName { get; }
        public IDictionary<int, OdbcConnection> Connections { get; }
        public string ConnectionString { get; }
        public Stopwatch Stopwatch { get; }

        private static Mutex mutex = new Mutex();
        private bool simulateDelay;

        public ReplayUnit(string connectionString, string databaseName, bool simulateDelay)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            Events = new List<DatabaseEvent>();
            Connections = new Dictionary<int, OdbcConnection>();
            Stopwatch = new Stopwatch();
            this.simulateDelay = simulateDelay;
        }

        public virtual void Reset()
        {
            ExecutedEvents = 0;
        }

        public virtual void Run(CancellationToken? token = null)
        {
            Stopwatch.Reset();
            try
            {
                for (int i = 0; i < Events.Count; i++)
                {
                    if (token != null && token.Value.IsCancellationRequested) break;

                    DatabaseEvent e = Events[i];
                    e.Execute();
                    ExecutedEvents = i + 1;

                    if (simulateDelay && i > 0)
                    {
                        DatabaseEvent prevEvent = Events[i - 1];
                        if ((prevEvent.EventType == DatabaseEvent.NONQUERY || prevEvent.EventType == DatabaseEvent.QUERY) &&
                            (e.EventType == DatabaseEvent.NONQUERY || e.EventType == DatabaseEvent.QUERY) &&
                            e.StartTime != null && prevEvent.StartTime != null)
                        {
                            double delay = (e.StartTime - prevEvent.StartTime).Value.TotalMilliseconds;
                            if (delay > 0) Thread.Sleep((int)delay);
                        }
                    }
                }
            }
            finally
            {
                foreach (OdbcConnection conn in Connections.Values)
                {
                    conn.Close();
                    conn.Dispose();
                }
                OdbcConnection.ReleaseObjectPool();
                Connections.Clear();
            }
        }

        private DatabaseEvent LastNonLogoutEvent(int i)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (Events[j].EventType != DatabaseEvent.AUDIT_LOGOUT) return Events[j];
            }
            return null;
        }

        public long RunTimeMillis { get { return Stopwatch.ElapsedMilliseconds; } }

        private int executedEvents;
        public int ExecutedEvents
        {
            get
            {
                mutex.WaitOne();
                int temp = executedEvents;
                mutex.ReleaseMutex();
                return temp;
            }
            protected set
            {
                mutex.WaitOne();
                executedEvents = value;
                mutex.ReleaseMutex();
            }
        }

        public int NbrEvents { get { return Events.Count; } }
    }
}

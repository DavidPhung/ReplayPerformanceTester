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
        public IDictionary<int, OdbcConnection> Connections { get; }
        public ConnectionInfo ConnectionInfo { get; }
        public Stopwatch Stopwatch { get; }

        private static Mutex mutex = new Mutex();

        public ReplayUnit(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
            Events = new List<DatabaseEvent>();
            Connections = new Dictionary<int, OdbcConnection>();
            Stopwatch = new Stopwatch();
        }

        public virtual void Reset()
        {
            ExecutedEvents = 0;
        }

        public virtual void Run(CancellationToken? token = null)
        {
            string connectionString = OdbcUtils.CreateConnectionString(ConnectionInfo);
            Stopwatch.Reset();
            try
            {
                for (int i = 0; i < Events.Count; i++)
                {
                    if (token != null && token.Value.IsCancellationRequested) break;

                    DatabaseEvent e = Events[i];
                    e.Execute();
                    ExecutedEvents = i + 1;
                }
            }
            finally
            {
                foreach (OdbcConnection conn in Connections.Values)
                    conn.Close();
            }
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

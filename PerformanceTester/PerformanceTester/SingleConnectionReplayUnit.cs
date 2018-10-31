using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Threading;

namespace PerformanceTester
{
    public class SingleConnectionReplayUnit : ReplayUnit
    {
        public OdbcConnection ExistingConnection { get; set; }

        public SingleConnectionReplayUnit(string connectionString, string databaseName) 
            : base(connectionString, databaseName, false)
        {

        }

        public SingleConnectionReplayUnit(string connectionString, string databaseName, OdbcConnection existingConnection)
            : base(connectionString, databaseName, false)
        {
            this.ExistingConnection = existingConnection;
        }

        public override void Run(CancellationToken? token)
        {
            Stopwatch.Reset();
            using (OdbcConnection conn = ExistingConnection ?? new OdbcConnection(ConnectionString))
            {
                if (conn != ExistingConnection) conn.Open();

                for (int i = 0; i < Events.Count; i++)
                {
                    if (token != null && token.Value.IsCancellationRequested) break;

                    DatabaseEvent e = Events[i];
                    if (e is QueryEvent || e is NonQueryEvent)
                    {
                        using (OdbcCommand cmd = new OdbcCommand(e.Text, conn))
                        {
                            Stopwatch.Start();
                            if (e.Text.Trim().Substring(0, "select".Length).ToLower().Equals("select"))
                                cmd.ExecuteReader().Close();
                            else
                                cmd.ExecuteNonQuery();
                            Stopwatch.Stop();
                        }
                    }
                    ExecutedEvents = i + 1;
                }
            }
        }
    }
}

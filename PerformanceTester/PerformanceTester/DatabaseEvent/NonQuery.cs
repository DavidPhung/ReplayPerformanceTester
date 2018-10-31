using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace PerformanceTester
{
    public class NonQueryEvent : DatabaseEvent
    {
        public NonQueryEvent(DatabaseEventExecutionContext context, int spid, string text, string databaseName, DateTime? startTime, long eventSequence)
            : base(context, NONQUERY, databaseName, spid, text, startTime, null, eventSequence)
        { }

        public override void Execute()
        {
            OdbcConnection conn = null;
            Context.Connections.TryGetValue(Spid, out conn);
            using (OdbcCommand command = new OdbcCommand(Text, conn))
            {
                command.CommandTimeout = 300;
                Context.Stopwatch.Start();
                command.ExecuteNonQuery();
                Context.Stopwatch.Stop();
            }
        }
    }
}

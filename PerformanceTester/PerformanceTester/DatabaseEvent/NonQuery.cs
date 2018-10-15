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
        public NonQueryEvent(DatabaseEventExecutionContext context, int spid, string text, string databaseName)
            : base(context, -1, databaseName, spid, text, null, null)
        { }

        public override void Execute()
        {
            OdbcConnection conn = null;
            Context.Connections.TryGetValue(Spid, out conn);
            using (OdbcCommand command = new OdbcCommand(Text, conn))
            {
                Context.Stopwatch.Start();
                command.ExecuteNonQuery();
                Context.Stopwatch.Stop();
            }
        }
    }
}

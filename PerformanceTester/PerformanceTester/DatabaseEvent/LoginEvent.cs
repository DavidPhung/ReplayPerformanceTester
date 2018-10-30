using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace PerformanceTester
{
    public class LoginEvent : DatabaseEvent
    {
        public LoginEvent(DatabaseEventExecutionContext context, int spid, string text, DateTime? startTime, long eventSequence)
            : base(context, AUDIT_LOGIN, "", spid, text, startTime, null, eventSequence)
        { }

        public override void Execute()
        {
            OdbcConnection conn = new OdbcConnection(Context.ConnectionString);
            conn.Open();
            Context.Connections.Add(Spid, conn);
        }
    }
}

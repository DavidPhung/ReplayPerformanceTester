using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace PerformanceTester
{
    public class LogoutEvent : DatabaseEvent
    {
        public LogoutEvent(DatabaseEventExecutionContext context, int spid, string text, DateTime? startTime, long eventSequence)
            : base(context, AUDIT_LOGOUT, "", spid, text, startTime, null, eventSequence)
        { }

        public override void Execute()
        {
            OdbcConnection conn = null;
            Context.Connections.TryGetValue(Spid, out conn);
            conn.Close();
            conn.Dispose();
            OdbcConnection.ReleaseObjectPool();
            Context.Connections.Remove(Spid);
        }
    }
}

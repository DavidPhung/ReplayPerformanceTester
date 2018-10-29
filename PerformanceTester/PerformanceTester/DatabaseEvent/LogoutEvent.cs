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
        public LogoutEvent(DatabaseEventExecutionContext context, int spid, string text)
            : base(context, -1, "", spid, text, null, null)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace PerformanceTester
{
    class DatabaseEventBuilder
    {
        public const int RPC_STARTING = 11;
        public const int SQL_BATCH_STARTING = 13;
        public const int AUDIT_LOGIN = 14;
        public const int AUDIT_LOGOUT = 15;
        public const int EXISTING_CONNECTION = 17;

        public static void Build(DatabaseEventExecutionContext context, DataTable traceTable)
        {
            IList<DatabaseEvent> list = context.Events;
            for (int i = 0; i < traceTable.Rows.Count; i++)
            {
                string text = traceTable.Rows[i]["TextData"].ToString();

                DateTime? startTime = null;
                object obj = traceTable.Rows[i]["StartTime"];
                if (!(obj is DBNull)) startTime = (DateTime)obj;

                DateTime? endTime = null;
                obj = traceTable.Rows[i]["EndTime"];
                if (!(obj is DBNull)) endTime = (DateTime)obj;

                string databaseName = "";
                obj = traceTable.Rows[i]["DatabaseName"];
                if (!(obj is DBNull)) databaseName = (string)obj;

                int spid = (int)traceTable.Rows[i]["SPID"];

                int eventClass = (int)traceTable.Rows[i]["EventClass"];

                long eventSequence = (long)traceTable.Rows[i]["EventSequence"];

                DatabaseEvent e = null;
                switch (eventClass)
                {
                    case AUDIT_LOGIN:
                        e = new LoginEvent(context, spid, text, startTime, eventSequence);
                        break;
                    case AUDIT_LOGOUT:
                        e = new LogoutEvent(context, spid, text, startTime, eventSequence);
                        break;
                    case EXISTING_CONNECTION:
                        e = new ExistingConnectionEvent(context, spid, text, startTime, eventSequence);
                        break;
                    case SQL_BATCH_STARTING:
                        if (text.Substring(0, 6).ToLower().Equals("select"))
                            e = new QueryEvent(context, spid, text, databaseName, startTime, eventSequence);
                        else
                            e = new NonQueryEvent(context, spid, text, databaseName, startTime, eventSequence);
                        break;
                    case RPC_STARTING:
                        e = new NonQueryEvent(context, spid, text, databaseName, startTime, eventSequence);
                        break;
                    default:
                        break;
                }
                list.Add(e);
            }
        }

        private void PrintEvents(List<DatabaseEvent> events)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < events.Count; i++)
            {
                DatabaseEvent e = events[i];
                sb.AppendLine(e.ToString());
                sb.AppendLine("---");
            }
            File.WriteAllText(@"C:\Users\ThePhuong\Desktop\out.txt", sb.ToString());
        }
    }
}

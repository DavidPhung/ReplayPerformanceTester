using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTester
{
    public abstract class DatabaseEvent
    {
        public const int NONQUERY = 1;
        public const int QUERY = 2;
        public const int AUDIT_LOGIN = 3;
        public const int AUDIT_LOGOUT = 4;
        public const int EXISTING_CONNECTION = 5;

        public int EventType { get; private set; }
        public string DatabaseName { get; private set; }
        public int Spid { get; private set; }
        public string Text { get; private set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public long EventSequence { get; private set; }

        public DatabaseEventExecutionContext Context { get; private set; }

        public DatabaseEvent(DatabaseEventExecutionContext context, int eventClass, string databaseName, 
            int spid, string text, DateTime? startTime, DateTime? endTime, long eventSequence)
        {
            Context = context;
            EventType = eventClass;
            Text = text;
            StartTime = startTime;
            EndTime = endTime;
            Spid = spid;
            DatabaseName = databaseName;
            EventSequence = eventSequence;
        }

        public abstract void Execute();

        public override string ToString()
        {
            string s = "Database event:\r\n";
            s += EventType + "\n";
            s += Spid + "\n";
            s += DatabaseName + "\n";
            s += Text + "\n";
            s += StartTime + "\n";
            s += EndTime.ToString();
            return s;
        }
    }
}

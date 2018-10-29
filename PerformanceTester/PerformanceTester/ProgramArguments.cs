using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PerformanceTester
{
    class ProgramArguments
    {
        public const string REPLAY_MODE_SINGLE_CONNECTION = "single";
        public const string REPLAY_MODE_MULTI_CONNECTION = "multi";
        public const string RESET_METHOD_SNAPSHOT = "snapshot";
        public const string RESET_METHOD_BACKUP = "backup";

        public string DriverName { get; protected set; }
        public string Server { get; protected set; }
        public string Database { get; protected set; }
        public string Dsn { get; protected set; }
        public string UserID { get; protected set; }
        public string Password { get; protected set; }

        public string ReplayMode { get; protected set; }
        public string ResetMethod { get; protected set; }
        public string Snapshot { get; protected set; }
        public string BackupFile { get; protected set; }
        public string TestDatabase { get; protected set; }
        public string SetupTraceFile { get; protected set; }
        public string TestTraceFile { get; protected set; }
        public string Process { get; protected set; }
        public int NbrRepeats { get; protected set; }
        public string OutputFile { get; protected set; }


        public static ProgramArguments ReadFromFile(string filename)
        {
            ProgramArguments args = new ProgramArguments();
            string[] lines = File.ReadAllLines(filename);
            args.DriverName = FindValue(lines, "Driver", "=").Trim();
            args.Database = FindValue(lines, "Database", "=").Trim();
            args.Server = FindValue(lines, "Server", "=").Trim();
            args.UserID = FindValue(lines, "UserID", "=").Trim();
            args.Password = FindValue(lines, "Password", "=").Trim();
            args.Dsn = FindValue(lines, "Dsn", "=").Trim();

            args.ReplayMode = FindValue(lines, "ReplayMode", "=").Trim().ToLower();
            args.ResetMethod = FindValue(lines, "ResetMethod", "=").Trim().ToLower();
            args.Snapshot = FindValue(lines, "Snapshot", "=").Trim();
            args.BackupFile = FindValue(lines, "BackupFile", "=").Trim();
            args.TestDatabase = FindValue(lines, "TestDatabase", "=").Trim();
            args.SetupTraceFile = FindValue(lines, "SetupTraceFile", "=").Trim();
            args.TestTraceFile = FindValue(lines, "TestTraceFile", "=").Trim();
            args.Process = FindValue(lines, "Process", "=").Trim();
            args.NbrRepeats = int.Parse(FindValue(lines, "NbrRepeats", "=").Trim());
            args.OutputFile = FindValue(lines, "OutputFile", "=").Trim();

            return args;
        }

        private static string FindValue(string[] lines, string fieldName, string delimeter)
        {
            foreach (string s in lines)
            {
                if (s.ToLower().StartsWith(fieldName.ToLower()))
                {
                    int d = s.IndexOf(delimeter);
                    return s.Substring(d + delimeter.Length);
                }
            }
            return "";
        }
    }
}

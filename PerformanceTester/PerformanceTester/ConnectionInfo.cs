using System;
using System.IO;
using System.Text;

namespace PerformanceTester
{
    public class ConnectionInfo
    {
        public string DbmsName          { get; set; }
        public string DriverName        { get; set; }
        public string Server            { get; set; }
        public string Database          { get; set; }
        public string UserID            { get; set; }
        public string Password          { get; set; }

        public string Dsn               { get; set; }

        public ConnectionInfo()
        {
            DbmsName = "";
            DriverName = "";
            Database = "";
            Server = "";
            UserID = "";
            Password = "";
            Dsn = "";
        }

        public ConnectionInfo(string filename)
        {
            ReadFromFile(filename);
        }

        public void ReadFromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            DbmsName = FindValue(lines, "DbmsName", "=").Trim();
            DriverName = FindValue(lines, "Driver", "=").Trim();
            Database = FindValue(lines, "Database", "=").Trim();
            Server = FindValue(lines, "Server", "=").Trim();
            UserID = FindValue(lines, "UserID", "=").Trim();
            Password = FindValue(lines, "Password", "=").Trim();
            Dsn = FindValue(lines, "Dsn", "=").Trim();
        }

        private string FindValue(string[] lines, string fieldName, string delimeter)
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

        public void WriteToFile(string filename)
        {
            string s = "DbmsName = " + DbmsName + Environment.NewLine
                + ", Driver = " + DriverName + Environment.NewLine
                + ", Server = " + Server + Environment.NewLine
                + ", Database = " + Database + Environment.NewLine
                + ", Dsn = " + Dsn + Environment.NewLine
                + ", UserId = " + UserID + Environment.NewLine
                + ", Password = " + Password;
        }

        public override string ToString()
        {
            return "DbmsName = " + DbmsName + ", Driver = " + DriverName + ", Server = " + Server + 
                ", Database = " + Database + ", Dsn = " + Dsn + ", UserId = " + UserID + ", Password = " + Password;
        }
    }
}

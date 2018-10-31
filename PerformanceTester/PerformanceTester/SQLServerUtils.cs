using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceTester
{
    class SQLServerUtils
    {
        public static void RestoreSnapshot(string snapshotName, string databaseName,string connectionString)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                //close existing connections by setting single user mode
                string s1 = "ALTER DATABASE " + databaseName +
                    " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

                //restore database from snapshot
                string s2 = "USE master;" +
                    "RESTORE DATABASE " + databaseName + 
                    " FROM DATABASE_SNAPSHOT = '" + snapshotName + "';"
                    ;

                //set back to multi user mode
                string s3 = "ALTER DATABASE " + databaseName +
                    " SET MULTI_USER WITH ROLLBACK IMMEDIATE;";

                OdbcUtils.ExecuteNonQuery(conn, s1 + s2 + s3);
            }
        }

        public static void RestoreFromBackup(string backupFile, string databaseName, string connectionString)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                //close existing connections by setting single user mode
                string s1 = "ALTER DATABASE " + databaseName + 
                    " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

                //restore from backup
                string s2 = "USE master;" +
                    "RESTORE DATABASE " + databaseName + 
                    " FROM DISK = '" + backupFile + "'" +
                    " WITH REPLACE;";

                //set back to multi user mode
                string s3 = "ALTER DATABASE " + databaseName + 
                    " SET MULTI_USER WITH ROLLBACK IMMEDIATE;";
                OdbcUtils.ExecuteNonQuery(conn, s1 + s2 + s3);
            }
        }
    }
}

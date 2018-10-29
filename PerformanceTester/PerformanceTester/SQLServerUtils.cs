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
                //kill existing connections
                string s1 = "USE master;" +
                    "DECLARE @kill varchar(8000) = ''; " +
                        "SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'" +
                    "FROM sys.dm_exec_sessions" +
                    " WHERE database_id = db_id('rpctst')" +
                    "EXEC(@kill); ";

                //restore database from snapshot
                string s2 = "USE master;" +
                    "RESTORE DATABASE rpctst FROM DATABASE_SNAPSHOT = '" + snapshotName + "';"
                    ;
                OdbcUtils.ExecuteNonQuery(conn, s1 + s2);
                OdbcUtils.ExecuteNonQuery(conn, "USE " + databaseName + ";");
            }
        }


    }
}

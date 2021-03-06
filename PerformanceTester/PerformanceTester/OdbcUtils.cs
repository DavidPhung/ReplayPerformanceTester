﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;

namespace PerformanceTester
{
    class OdbcUtils
    {
        public static string CreateConnectionString(ProgramArguments args)
        {
            if (!args.Dsn.Equals(""))
                return "Dsn=" + args.Dsn + ";Uid=" + args.UserID +
                    ";Pwd=" + args.Password + ";";

            return "Driver={" + args.DriverName
                + "}; server=" + args.Server
                + "; database=" + args.Database
                + "; Uid=" + args.UserID
                + ";Pwd=" + args.Password
                + ";";
        }

        public static object ExecuteScalar(OdbcConnection conn, string sql)
        {
            object o = null;
            using (OdbcCommand command = new OdbcCommand(sql, conn))
            {
                o = command.ExecuteScalar();
            }
            return o;
        }

        public static int ExecuteNonQuery(OdbcConnection conn, string sql)
        {
            int nbrRowsAffected = 0;
            using (OdbcCommand command = new OdbcCommand(sql, conn))
            {
                nbrRowsAffected = command.ExecuteNonQuery();
            }
            return nbrRowsAffected;
        }

        public static DataTable ExecuteReader(OdbcConnection conn, string sql, bool returnResult = true)
        {
            DataTable dataTable = new DataTable();
            using (OdbcCommand command = new OdbcCommand(sql, conn))
            {
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    if (returnResult)
                    {
                        using (DataSet ds = new DataSet() { EnforceConstraints = false })
                        {
                            ds.Tables.Add(dataTable);
                            dataTable.Load(reader);
                            ds.Tables.Remove(dataTable);
                        }
                    }
                }
            }
            return dataTable;
        }
    }
}

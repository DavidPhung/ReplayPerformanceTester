using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace PerformanceTester
{
    /// <summary>
    /// Runs SQL procedure on SQL database.
    /// </summary>
    public class SQLServerProcedureUtils
    {

        public static void ExecuteProcedureFromText(OdbcConnection connection, string text)
        {
            using (OdbcCommand command = new OdbcCommand(text, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static int ExecuteProcedure(OdbcConnection connection, string procedureName, string[] paramNames, OdbcType[] types, object[] values, List<object> outParams = null)
        {
            int retcode = -1;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paramNames.Length; i++)
            {
                sb.Append("?");
                if (i < paramNames.Length - 1) sb.Append(",");
            }
            string sql = "{? = CALL " + procedureName + " (" + sb.ToString() + ")}";
            using (OdbcCommand command = new OdbcCommand(sql, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                OdbcParameter returnValue = new OdbcParameter("@return_value", OdbcType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);
                for (int i = 0; i < paramNames.Length; i++)
                {
                    OdbcParameter param = command.CreateParameter();
                    param.ParameterName = "@" + paramNames[i];
                    param.OdbcType = types[i];
                    if (values[i] == null) param.Direction = ParameterDirection.Output;
                    else param.Value = values[i];
                    command.Parameters.Add(param);
                }
                command.ExecuteNonQuery();
                retcode = (int)command.Parameters["@return_value"].Value;
                for (int i = 0; i < paramNames.Length; i++)
                {
                    if (values[i] == null && outParams != null) outParams.Add(command.Parameters["@" + paramNames[i]].Value);
                }
            }

            return retcode;
        }

        public static int ExecuteProcedure(OdbcConnection connection, string procedureName, List<OdbcParameter> parameters, List<object> outParams = null)
        {
            int retcode = -1;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Count; i++)
            {
                sb.Append("?");
                if (i < parameters.Count - 1) sb.Append(",");
            }
            string sql = "{? = CALL " + procedureName + " (" + sb.ToString() + ")}";
            using (OdbcCommand command = new OdbcCommand(sql, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                OdbcParameter returnValue = new OdbcParameter("@return_value", OdbcType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);
                for (int i = 0; i < parameters.Count; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
                command.ExecuteNonQuery();
                retcode = (int)command.Parameters["@return_value"].Value;
                if (outParams != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        if (parameters[i].Direction == ParameterDirection.Output)
                            outParams.Add(command.Parameters["@" + parameters[i].ParameterName].Value);
                    }
                }
            }

            return retcode;
        }

        public static int ExecuteTraceCreate(OdbcConnection connection, out int traceid, int options, string traceFilename)
        {
            List<object> outParams = new List<object>();
            int retcode = SQLServerProcedureUtils.ExecuteProcedure(connection, "sp_trace_create",
                new string[] { "traceid", "options", "tracefile" },
                new OdbcType[] { OdbcType.Int, OdbcType.Int, OdbcType.NVarChar },
                new object[] { null, 2, traceFilename },
                outParams);

            traceid = (int)outParams[0];
            return retcode;
        }

        public static int ExecuteTraceSetEvent(OdbcConnection connection, int traceid, int eventId, int columnid, int on)
        {
            List<object> outParams = new List<object>();
            int retcode = SQLServerProcedureUtils.ExecuteProcedure(connection, "sp_trace_setevent",
                new string[] { "traceid", "eventid", "columnid", "on" },
                new OdbcType[] { OdbcType.Int, OdbcType.Int, OdbcType.Int, OdbcType.Bit },
                new object[] { traceid, eventId, columnid, on },
                outParams);
            return retcode;
        }

        public static int ExecuteTraceSetStatus(OdbcConnection connection, int traceid, int status)
        {
            List<object> outParams = new List<object>();
            int retcode = SQLServerProcedureUtils.ExecuteProcedure(connection, "sp_trace_setstatus",
                new string[] { "traceid", "status" },
                new OdbcType[] { OdbcType.Int, OdbcType.Int },
                new object[] { traceid, status },
                outParams);
            return retcode;
        }

        
    }
}

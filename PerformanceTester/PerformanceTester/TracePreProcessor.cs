using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace PerformanceTester
{
    class TracePreprocessor
    {
        public static void Preprocess(DataTable traceTable, string targetDatabase)
        {
            //RemoveResetConnectionProceduresAroundLogout(traceTable);
            RemoveAllResetConnectionProcedures(traceTable);
            RemoveUnrelatedServerProcesses(traceTable, targetDatabase);
            ReplaceCursorEventsWithNormalEvents(traceTable);
        }

        public static void RemoveResetConnectionProceduresAroundLogout(DataTable traceTable)
        {
            for (int i = 1; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                int eventClass = (int)row["EventClass"];
                DataRow prevRow = traceTable.Rows[i - 1];
                string prevText = prevRow["TextData"].ToString();
                if (eventClass == DatabaseEventBuilder.AUDIT_LOGOUT 
                    && prevText.Trim().ToLower().Equals("exec sp_reset_connection"))
                {
                    prevRow.Delete();
                }
            }
            traceTable.AcceptChanges();
        }

        public static void RemoveAllResetConnectionProcedures(DataTable traceTable)
        {
            for (int i = 1; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                string text = row["TextData"].ToString();
                if (text.Trim().ToLower().Equals("exec sp_reset_connection"))
                {
                    row.Delete();
                }
            }
            traceTable.AcceptChanges();
        }

        private static void RemoveUnrelatedServerProcesses(DataTable traceTable, string targetDatabase)
        {
            //Group rows from same connection together
            List<List<DataRow>> connections = new List<List<DataRow>>();
            Dictionary<int, int> spidDict = new Dictionary<int, int>();

            for (int i = 0; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                int eventClass = (int)row["EventClass"];
                int spid = (int)row["SPID"];
                if (eventClass == DatabaseEventBuilder.AUDIT_LOGIN || eventClass == DatabaseEventBuilder.EXISTING_CONNECTION)
                {
                    List<DataRow> l = new List<DataRow>();
                    l.Add(row);
                    connections.Add(l);
                    spidDict.Add(spid, connections.Count - 1);
                }else if (eventClass == DatabaseEventBuilder.AUDIT_LOGOUT)
                {
                    int index = spidDict[spid];
                    connections[index].Add(row);
                    spidDict.Remove(spid);
                }else
                {
                    int index = spidDict[spid];
                    connections[index].Add(row);
                }
            }

            /*
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < connections.Count; i++)
            {
                sb.AppendLine("Group " + i + " ");
                for (int j = 0; j < connections[i].Count; j++)
                {
                    DataRow row = connections[i][j];
                    sb.Append("\t");
                    sb.Append(row["EventClass"] + ",");
                    sb.Append(row["EventSequence"] + ",");
                    sb.Append(row["SPID"] + ",");
                    sb.Append(row["DatabaseName"] + ",");
                    sb.AppendLine();
                }
            }
            //Console.WriteLine(sb.ToString());
            File.WriteAllText(@"C:\Users\ThePhuong\Desktop\out.txt", sb.ToString());
            */

            //Check and remove connections that have nothing to do with target database
            List<int> connectionsToRemove = new List<int>();
            for (int i = 0; i < connections.Count; i++)
            {
                bool remove = true;
                foreach (var row in connections[i])
                {
                    int eventClass = (int)row["EventClass"];
                    if (eventClass != DatabaseEventBuilder.AUDIT_LOGIN
                        && eventClass != DatabaseEventBuilder.EXISTING_CONNECTION
                        && eventClass != DatabaseEventBuilder.AUDIT_LOGOUT
                        && row["DatabaseName"].Equals(targetDatabase))
                    {
                        remove = false;
                        break;
                    }
                }
                if (remove) connectionsToRemove.Add(i);
            }

            foreach (int i in connectionsToRemove)
            {
                foreach (var row in connections[i])
                {
                    row.Delete();
                }
            }
            traceTable.AcceptChanges();
        }

        private static void ReplaceCursorEventsWithNormalEvents(DataTable traceTable)
        {
            for (int i = 1; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                string text = row["TextData"].ToString();
                if (text.Contains("exec sp_cursoropen")
                    || text.Contains("exec sp_cursorprepexec")
                    || text.Contains("exec sp_cursorexecute"))
                {
                    int f = text.IndexOf('\'');
                    int l = text.LastIndexOf('\'');
                    string newText = text.Substring(f + 1, l - f - 1);
                    newText = newText.Replace("''", "'");
                    row.SetField("TextData", newText);
                }else if (text.Contains("exec sp_cursor")) 
                {
                    row.Delete();
                }
            }
            traceTable.AcceptChanges();
        }
    }
}

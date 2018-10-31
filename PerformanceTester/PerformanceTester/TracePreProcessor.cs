using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace PerformanceTester
{
    class TracePreprocessor
    {
        public static void Preprocess(DataTable traceTable, string targetDatabase)
        {
            //RemoveResetConnectionProceduresAroundLogout(traceTable);
            RemoveAllResetConnectionProcedures(traceTable);
            RemoveEventsThatDoNotTargetTestDatabase(traceTable, targetDatabase);
            ReplaceCursorEventsWithNormalEvents(traceTable);
            ReplacePrepareEventsWithNormalEvents(traceTable);
        }

        public static void PrintTrace(DataTable traceTable)
        {
            for (int i = 0; i < 10; i++)
            {
                DataRow row = traceTable.Rows[i];
                int eventClass = (int)row["EventClass"];
                string text = row["TextData"].ToString();
                long eventSeq = (long)row["EventSequence"];

                Debug.WriteLine("# {0} {1}", eventClass, eventSeq);
            }
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

        private static void RemoveEventsThatDoNotTargetTestDatabase(DataTable traceTable, string targetDatabase)
        {
            for (int i = 0; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                string databaseName = row["DatabaseName"].ToString();
                string textData = row["TextData"].ToString();
                if (!databaseName.Equals(targetDatabase) &&
                    !textData.Trim().ToLower().Contains("use " + targetDatabase))
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

        private static void ReplacePrepareEventsWithNormalEvents(DataTable traceTable)
        {
            for (int i = 0; i < traceTable.Rows.Count; i++)
            {
                DataRow row = traceTable.Rows[i];
                string text = row["TextData"].ToString();
                if (text.Contains("exec sp_prepexec"))
                {
                    int f = text.IndexOf('\'');
                    int l = text.LastIndexOf('\'');
                    string newText = text.Substring(f + 1, l - f - 1);
                    newText = newText.Replace("''", "'");
                    row.SetField("TextData", newText);
                }
                else if (text.Contains("exec sp_unprepare"))
                {
                    row.Delete();
                }
            }
            traceTable.AcceptChanges();
        }
    }
}

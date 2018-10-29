using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace PerformanceTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Read command arguments
            ProgramArguments arguments = ProgramArguments.ReadFromFile(args[0]);

            GUIDataMonitor monitor = new GUIDataMonitor();
            ReplayManager replayManager = new ReplayManager(arguments, monitor);

            var task = Task.Run(() => RunReplayManager(replayManager, arguments.NbrRepeats, arguments.OutputFile));

            Thread guiThread = new Thread(() => DisplayGUI(monitor));
            guiThread.SetApartmentState(ApartmentState.STA);
            guiThread.Start();

            task.Wait();
            guiThread.Join();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void DisplayGUI(GUIDataMonitor monitor)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(monitor));
        }

        private static void RunReplayManager(ReplayManager replayManager, int nbrRepeats, string outputFile)
        {
            replayManager.Run(nbrRepeats);
            Console.WriteLine("----------");
            for (int i = 0; i < replayManager.RunTimeMillis.Count; i++)
            {
                Console.WriteLine("#" + (i + 1) + ": " + "respond time = " + replayManager.RunTimeMillis[i]
                    + " average memory = " + replayManager.MemReaders[i].GetAverage());
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Run, Reponse Time (ms), Average Memory (MB)");
            for (int i = 0; i < replayManager.RunTimeMillis.Count; i++)
            {
                sb.Append((i + 1));
                sb.Append(",");
                sb.Append(replayManager.RunTimeMillis[i]);
                sb.Append(",");
                sb.Append(replayManager.MemReaders[i].GetAverage());
                sb.AppendLine();
            }
            File.WriteAllText(outputFile, sb.ToString());
            Console.WriteLine("Output saved to " + outputFile);
        }
    }
}

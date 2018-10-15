using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTester
{
    class MemoryReader
    {

        private List<int> measurements;
        private string processName;
        private bool end = false;
        private System.Threading.Thread thread;

        public MemoryReader(string processName)
        {
            this.processName = processName;
        }

        private void Record()
        {
            while (!end)
            {
                RecordMemoryUsage();
                System.Threading.Thread.Sleep(200);
            }
        }

        public void StartMeasure()
        {
            measurements = new List<int>();
            end = false;
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(Record));
            thread.Start();
        }

        public void EndMeasure()
        {
            end = true;
        }

        private void RecordMemoryUsage()
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessesByName(processName)[0];
            int memsize = 0; // memsize in Megabyte
            PerformanceCounter PC = new PerformanceCounter();
            PC.CategoryName = "Process";
            PC.CounterName = "Working Set - Private";
            PC.InstanceName = proc.ProcessName;
            memsize = (int)(PC.NextValue() / (int)(1024));
            measurements.Add(memsize);
            PC.Close();
            PC.Dispose();
        }
        public int[] GetMeasurements()
        {
            return measurements.ToArray();
        }

        public long GetAverage()
        {
            long sum = 0;
            foreach (int m in measurements)
            {
                sum += m;
            }
            if (measurements.Count != 0) sum /= measurements.Count;
            return sum;
        }

        public long GetMax()
        {
            long max = 0;
            foreach (int m in measurements)
            {
                if (m > max) max = m;
            }
            return max;
        }
    }

}

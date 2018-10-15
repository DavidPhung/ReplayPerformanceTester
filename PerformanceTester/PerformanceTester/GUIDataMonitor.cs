using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceTester
{
    public class GUIDataMonitor
    {
        private object lockObj;
        private List<long> runTimeMillis;
        private List<double> avgMems;

        public GUIDataMonitor()
        {
            lockObj = new object();
            runTimeMillis = new List<long>();
            avgMems = new List<double>();
        }

        public void AddRunTimeMillis(long value)
        {
            lock (lockObj)
            {
                runTimeMillis.Add(value);
                Monitor.PulseAll(lockObj);
            }
        }

        public void AddAvgMem(long value)
        {
            lock (lockObj)
            {
                avgMems.Add(value);
                Monitor.PulseAll(lockObj);
            }
        }

        public List<long> GetLatestRunTimeMillis(ref int index)
        {
            lock (lockObj)
            {
                while (index >= runTimeMillis.Count) Monitor.Wait(lockObj);
                List<long> l = runTimeMillis.GetRange(index, runTimeMillis.Count - index);
                index = runTimeMillis.Count;
                return l;
            }
        }

        public List<double> GetLatestAvgMems(ref int index)
        {
            lock (lockObj)
            {
                while (index >= avgMems.Count) Monitor.Wait(lockObj);
                List<double> l = avgMems.GetRange(index, avgMems.Count - index);
                index = avgMems.Count;
                return l;
            }
        }
    }
}

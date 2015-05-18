using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TaskManager
{
    class ProcessToView
    {
        public string Name { set; get; }
        public int ID { set; get; }
        public int CountThreads { set; get; }
        public double MemoryMB { set; get; }
        public string CPUUsage { set; get; }

        public ProcessToView(int id, String Name, int CountThreads, double MemoryMB, string CPUUsage)
            {
                this.Name = Name;
                this.ID = id;
                this.CountThreads = CountThreads;
                this.MemoryMB = MemoryMB;
                this.CPUUsage = CPUUsage;
            }
        public static List<ProcessToView> GetProcesses()
        {
            List<Process> listP = Process.GetProcesses().ToList();
            var performanceCounters = new List<PerformanceCounter>();
            List<ProcessToView> ListFinal = new List<ProcessToView>();
            foreach (var e in listP)
                {
                    performanceCounters.Add(new PerformanceCounter("Process", "% Processor Time", e.ProcessName));
                    var tmp = performanceCounters[performanceCounters.Count - 1].NextValue().ToString("F2");
                    ListFinal.Add(new ProcessToView(e.Id, e.ProcessName, e.Threads.Count, Math.Round(((e.WorkingSet64 / 1024.0f) / 1024.0f), 2), tmp));
                }
            return ListFinal;
        }
        
    }
}

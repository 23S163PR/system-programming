using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManeger
{
    public class TaskManager
    {
        List<SystemProcess> _items;
     //   List<PerformanceCounter> _performanceCounters;

        public TaskManager()
        {
            _items = new List<SystemProcess>();
           //_performanceCounters = new List<PerformanceCounter>();

           // foreach (var process in Process.GetProcesses())
           // {
           //     _performanceCounters.Add(new PerformanceCounter("Process", "% Processor Time", process.ProcessName));
           // }
        }

        public List<SystemProcess> GetProcessList()
        {
            _items.Clear();
            foreach (var process in Process.GetProcesses())
            {
                _items.Add(new SystemProcess(process.Id, process.ProcessName, process.Threads.Count, (int)process.WorkingSet64 / 1024, 4));             
            }

            return _items;
        }


        public void EndProcess(int id)
        {
           Process.GetProcessById(id).Kill();
        }
    }
}

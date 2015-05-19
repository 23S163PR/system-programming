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

        public TaskManager()
        {
            _items = new List<SystemProcess>();
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

        public void ChangepPriority(int id, ProcessPriorityClass priority)
        {
            Process.GetProcessById(id).PriorityClass = priority;
        }

        public ProcessPriorityClass GetPrioruteProcess(int id)
        {
            return Process.GetProcessById(id).PriorityClass;
        }
    }
}

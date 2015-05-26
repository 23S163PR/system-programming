using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Collections.ObjectModel; 

namespace TaskManeger
{
    public class TaskManager
    {
        ObservableCollection<SystemProcess> _items;
        public ObservableCollection<SystemProcess> Foos { get { return _items; } }

        public TaskManager()
        {
            _items = new ObservableCollection<SystemProcess>();
        }

        public ObservableCollection<SystemProcess> GetProcessList()
        {
            _items.Clear();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");
            var cpuinfo = searcher.Get()
                .Cast<ManagementObject>()
                .Where(obj => obj["Name"].ToString() != "Idle")
                .Where(obj => obj["Name"].ToString() != "_Total")
                .ToArray();

            foreach (ManagementObject process in cpuinfo)  
            {
                _items.Add(new SystemProcess
                {
                      Id = int.Parse(process["IDProcess"].ToString())
                    , Name = process["Name"].ToString()
                    , Threads = process["ThreadCount"].ToString()
                    , Memory = String.Format("{0:f} K", (int.Parse(process["WorkingSet"].ToString()) / 1024F) / 1024F)
                    , CPU = String.Format("{0} % ", process["PercentProcessorTime"].ToString())
                });
  
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
          public ProcessPriorityClass GetPridsadoruteProcess(int id)
        {
            return Process.GetProcessById(id).PriorityClass;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management; 

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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");
            var cpuinfo = searcher.Get()
                .Cast<ManagementObject>()
                .Where(obj => obj["Name"].ToString() != "Idle")
                .Where(obj => obj["Name"].ToString() != "_Total")
                .ToArray();

            foreach (ManagementObject process in cpuinfo)  
            {
                _items.Add(new SystemProcess(
                    int.Parse(process["IDProcess"].ToString())
                    , process["Name"].ToString()
                    , process["ThreadCount"].ToString()
                    , String.Format("{0:f} K", (int.Parse(process["WorkingSet"].ToString()) / 1024F) / 1024F)
                    , String.Format("{0} % ", process["PercentProcessorTime"].ToString())
                    )); 
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

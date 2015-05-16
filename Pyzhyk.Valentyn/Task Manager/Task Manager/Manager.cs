using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Task_Manager
{
    public class Manager
    {
        public List<ProcessModel> Processes;
        public static List<ProcessModel> GetProcess()
        {
            List<ProcessModel> processes = new List<ProcessModel>();

            foreach (var process in Process.GetProcesses())
            {
                if (process.Id == 0) continue;
                try
                {
                    processes.Add(new ProcessModel(process));
                }
                catch (Exception e)
                { }
            }
            return processes;
        }
        public static void UpdateProcessesInfo(DataGrid ListProcesses)
        {
            foreach (var item in ListProcesses.Items)
            {
                try
                {
                    var newProcess = Process.GetProcessById(((ProcessModel)item).ProcessId);
                    ((ProcessModel)item).SetInfoProcess(newProcess);
                }
                catch (Exception)
                {
                    
                }
            }
        }
        public static bool CheckToAddNewProcess(DataGrid ListProcesses)
        {
            bool flag;
            foreach (var process in Process.GetProcesses())
            {
                flag = true;
                foreach (var item in ListProcesses.Items)
                {
                    if (((ProcessModel)item).ProcessId == process.Id)
                    {
                        flag = false;
                    }
                }
                if (flag && process.ProcessName != "Idle")
                {
                    return true;
                }
            }
            return false;
        }
    }
}

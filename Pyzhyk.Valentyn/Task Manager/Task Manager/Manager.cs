using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class Manager
    {
        public static List<ProcessModel> GetProcess()
        {
            List<ProcessModel> processes = new List<ProcessModel>();
            foreach (var process in Process.GetProcesses())
            {
                if (process.Id == 0) continue;

                if (process.ProcessName != "Idle")
                {
                    // WorkingSet64 don't overlap with Windows Task manager
                    try
                    {
                        processes.Add(new ProcessModel(
                            process.ProcessName,
                            process.Threads.Count,
                            //process.UserProcessorTime.Milliseconds,
                            process.WorkingSet64,
                            new PerformanceCounter("Process", "% Processor Time", process.ProcessName).NextValue().ToString("F2"),
                            process.Id));
                    }
                    catch (Exception e)
                    { }
                }
            }
            return processes;
        }
        
    }
}

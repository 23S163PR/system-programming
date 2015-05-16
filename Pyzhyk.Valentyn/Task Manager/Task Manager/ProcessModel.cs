using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class ProcessModel 
    {
        public string Name { get; set; }
        public int Thread { get; set; }
        public string Memory { get; set; }
        public string Cpu { get; set; }
        public int ProcessId { get; set; }

        public ProcessModel(string name, int thread, float memory, TimeSpan cpu, int processId)
        {
            Name = name;
            Thread = thread;
            Cpu = cpu.Milliseconds.ToString("F1");
            ProcessId = processId;
            Memory = ConvertMemory.ShorteningMemory(memory);
        }
        public ProcessModel(Process process)
        {
            Name = process.ProcessName;
            Thread = process.Threads.Count;
            try
            {
                Cpu = process.UserProcessorTime.Milliseconds.ToString("F1");
            }
            catch (Exception)
            {
                Cpu = "none";
            }
            
            ProcessId = process.Id;
            Memory = ConvertMemory.ShorteningMemory(process.WorkingSet64);
        }
        public void SetInfoProcess(int thread, float memory, TimeSpan cpu)
        {
            Thread = thread;
            Cpu = cpu.Milliseconds.ToString("F1");
            Memory = ConvertMemory.ShorteningMemory(memory);
        }
        public void SetInfoProcess(Process process)
        {
            Thread = process.Threads.Count;
            try
            {
                Cpu = process.UserProcessorTime.Milliseconds.ToString("F1");
            }
            catch (Exception)
            {
                Cpu = "none";
            }
            Memory = ConvertMemory.ShorteningMemory(process.WorkingSet64);
        }
    }
}

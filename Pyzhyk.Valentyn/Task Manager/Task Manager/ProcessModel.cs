using System;
using System.Collections.Generic;
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
        public int ProcID { get; set; }

        public ProcessModel(string name, int thread, float memory, string cpu, int procID)
        {
            Name = name;
            Thread = thread;
            Cpu = cpu;
            ProcID = procID;
            //Memory = memory;
            Memory = ConvertMemory.ShorteningMemory(memory);
        }
    }
}

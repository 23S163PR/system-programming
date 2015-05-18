using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManeger
{
    public class SystemProcess
    {
        public int Id { private set; get; }
        public string Name {private set; get; }
        public int Threads {private set; get; }
        public int Memory  {private set; get; }
        public int CPU     {private set; get; }


        public SystemProcess(int id,string name, int threads, int memory, int cpu)
        {
            Id = id;
            Name = name;
            Threads = threads;
            Memory = memory;
            CPU = cpu;
        }

    }
}

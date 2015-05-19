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
        public string Threads { private set; get; }
        public string Memory { private set; get; }
        public string CPU { private set; get; }


        public SystemProcess(int id, string name, string threads, string memory, string cpu)
        {
            Id = id;
            Name = name;
            Threads = threads;
            Memory = memory;
            CPU = cpu;
        }

    }
}

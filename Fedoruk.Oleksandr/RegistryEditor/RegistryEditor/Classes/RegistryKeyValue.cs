using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryEditor.Classes
{
    public class RegistryKeyValue
    {
        public String Name { get; private set; }
        public String Type { get; private set; }
        public String Data { get; private set; }


        public RegistryKeyValue(String name, String type, String data)
        {
            Name = name;
            Type = type;
            Data = data;
        }

    }
}

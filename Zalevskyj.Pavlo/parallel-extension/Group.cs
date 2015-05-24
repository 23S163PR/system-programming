using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    public class Group
    {
        [XmlArray("employees"), XmlArrayItem("employee")]
        public List<Employee> group { get; set; }

        public string NameGroup { get; set; }
        public Group()
        {
            group = new List<Employee>();
        }

        public Group(string nameGroup, List<Employee> list)
        {
            NameGroup = nameGroup;
            group = list;
        }
    }
}

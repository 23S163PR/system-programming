using System.Collections.Generic;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    [XmlRoot("groop")]
    public class Groups
    {
        [XmlArray("employees"), XmlArrayItem("employee")]
        public List<Employee> Colection { get; set; }
        [XmlAttribute("employeeCount")]
        public long Count { get; set; }

        public Groups() { }

        public Groups(List<Employee> list)
        {
            Colection = list; //.AddRange(list);
            Count = Colection.Count;
        }
        
    }
}

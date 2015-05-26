using System.Collections.Generic;
using System.Linq;
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

        public Groups(IEnumerable<Employee> list)
        {
            Colection = new List<Employee>();
            Colection.AddRange(list);
            Count = Colection.Count();
        }
        
    }
}

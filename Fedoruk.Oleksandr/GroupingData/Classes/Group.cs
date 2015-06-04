using System.Collections.Generic;
using System.Xml.Serialization;

namespace grouping_data.Classes
{
    [XmlRoot("groop")]
    public class Group
    {
        [XmlArray("employees"), XmlArrayItem("employee")]
        public List<Employee> list { get; set; }

        public Group()
        {
            list = new List<Employee>();
        }
        public Group(List<Employee> source)
        {
            list = source;
        }
    }
}

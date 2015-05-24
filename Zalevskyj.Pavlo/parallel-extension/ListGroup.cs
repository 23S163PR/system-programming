using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    public class ListGroups
    {
        [XmlArray("employees"), XmlArrayItem("employee")]
        public List<Group> Colection { get; set; }

        public ListGroups()
        {
            Colection = new List<Group>();
        }
    }
}

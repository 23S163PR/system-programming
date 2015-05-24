using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
     [Serializable]
    public class Employee
    {
        [XmlElement("Identity")]
        public Guid Identity { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("AgeInYears")]
        public int AgeInYears { get; set; }

        [XmlElement("Gender")]
        public Gender Gender { get; set; }

        [XmlElement("Salary")]
        public decimal Salary { get; set; }
    }

    public enum Gender
    {
        Men,
        Woman,
        Transgender
    }
}

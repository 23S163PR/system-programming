using grouping_data.Enums;
using System;
using System.Xml.Serialization;

namespace grouping_data.Classes
{
    [Serializable]
    [XmlRoot("employee")]
    public class Employee
    {
        [XmlElement(ElementName = "identity")]
        public Guid Identity { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "ageInYears")]
        public int AgeInYears { get; set; }
        [XmlElement(ElementName = "gender")]
        public Gender Gender { get; set; }
        [XmlElement(ElementName = "salary")]
        public decimal Salary { get; set; }
    }


}

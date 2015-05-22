using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_extension_demo
{
    public class Employee
    {
        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AgeInYears { get; set; }
        public Gender Gender { get; set; }
        public decimal Salary { get; set; }
    }

    public enum Gender
    {
        Men,
        Woman,
        Transgender
    }
}

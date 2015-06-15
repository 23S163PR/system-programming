using System.Collections.Generic;
using System.Linq;

namespace Groups
{
    public enum FieldEmployees
    {
        Identity,
        Name,
        Email,
        AgeInYears,
        Gender,
        Salary
    }

    public static class EmployeesExtension
    {
        public static List<Employee> SortEmployees(this IEnumerable<Employee> employees, FieldEmployees field, bool ascending)
        {
            switch (field)
            {
                case FieldEmployees.Name:
                    employees = @ascending ? employees.OrderBy(e => e.Name).ToList() : employees.OrderByDescending(e => e.Name).ToList();
                    break;
                case FieldEmployees.AgeInYears:
                    employees = @ascending ? employees.OrderBy(e => e.AgeInYears).ToList() : employees.OrderBy(e => e.AgeInYears).ToList();
                    break;
                case FieldEmployees.Email:
                    employees = @ascending ? employees.OrderBy(e => e.Email).ToList() : employees.OrderByDescending(e => e.Email).ToList();
                    break;
                case FieldEmployees.Gender:
                    employees = @ascending ? employees.OrderBy(e => e.Gender).ToList() : employees.OrderByDescending(e => e.Gender).ToList();
                    break;
                case FieldEmployees.Identity:
                    employees = @ascending ? employees.OrderBy(e => e.Identity).ToList() : employees.OrderByDescending(e => e.Identity).ToList();
                    break;
                case FieldEmployees.Salary:
                    employees = @ascending ? employees.OrderBy(e => e.Salary).ToList() : employees.OrderByDescending(e => e.Salary).ToList();
                    break;
            }
            return employees.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Faker;

namespace Groups
{
    public static class ManageEmployees
    {
        static Random _rand = new Random();

        public static List<Employee> ParseOfRealData(List<string> names)
        {
            var list = new List<Employee>(names.Count);
            list.AddRange(names.AsParallel().Select(name => new Employee
            {
                Identity = Guid.NewGuid(),
                Name = name,
                Email = name.GetEmail(),
                Gender = name.GetGender(),
                AgeInYears = _rand.Next(18, 60),
                Salary = _rand.Next(1800, 50000)
            }));
            return list;
        }

        public static List<Employee> CreateMythicalData(int count)
        {
            var list = new List<Employee>(count);
            Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, i =>
            {
                var name = Name.FullName();
                var employee = new Employee
                {
                    Identity = Guid.NewGuid(),
                    Name = name,
                    Email = Internet.Email(name),
                    AgeInYears = RandomNumber.Next(25, 80),
                    Salary = RandomNumber.Next(10000, 30000),
                    Gender = Gender.Transgender
                };
                list.Add(employee);
            });
            return list;
        }

        public static void SavetoXml(string pathFile, List<Employee> elements)
        {                                 
            var xmlSerializer = new XmlSerializer(typeof(List<Employee>));
            using (var fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                xmlSerializer.Serialize(fs, elements);
            }
        }

        public static List<Employee> DownloadFromXml(string pathFile)
        {
            List<Employee> employees;
            var mySerializer = new XmlSerializer(typeof(List<Employee>));
            using (var fs = new FileStream(pathFile, FileMode.Open))
            {
                employees = (List<Employee>)mySerializer.Deserialize(fs);
            }
            return employees;
        }
    }
}

using grouping_data.Classes;
using grouping_data.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace grouping_data
{

    class Program
    {

        static List<Employee> GetEmployees(int count)
        {
            var list = new List<Employee>(count);
            Parallel.For(0, count, i =>
            {
                var name = Faker.Name.FullName();
                var employee = new Employee
                {
                    Identity = Guid.NewGuid(),
                    Name = name,
                    Email = Faker.Internet.Email(name),
                    AgeInYears = Faker.RandomNumber.Next(18, 80),
                    Salary = Faker.RandomNumber.Next(10000, 30000), // $
                    Gender = Faker.Extensions.EnumExtensions.Rand<Gender>()
                };

                list.Add(employee);
            });

            return list;
        }
        static void Serrialization(List<Group> listGroups)
        {
            for (int i = 0; i < listGroups.Count; i++)
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(listGroups[i].GetType());
                    var path = @"files/group" + (i + 1) + ".xml";
                    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        xmlSerializer.Serialize(fs, listGroups[i]);
                        fs.Flush();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            const int PeopleCount = 20000;
            var people = GetEmployees(PeopleCount);

            var firstGroup  = new Group(people.Where(x => x.AgeInYears > 21 && x.Salary > 15000).OrderBy(x => x.AgeInYears).ToList());
            var secondGroup = new Group(people.Where(x => x.Gender == Gender.Woman && x.Name[0] == 'A').OrderBy(x => x.AgeInYears).ToList());
            var thirdGroup  = new Group(people.Where(x => x.Gender == Gender.Men && x.Salary > 20000).OrderBy(x => x.AgeInYears).ToList());
            var fourthGroup = new Group(people.Where(x => x.Gender == Gender.Transgender && x.AgeInYears > 50).OrderBy(x => x.AgeInYears).ToList());

            var listGroups = new List<Group>()
            {
                firstGroup
                , secondGroup
                , thirdGroup
                , fourthGroup
            };

            Serrialization(listGroups);

            stopWatch.Stop();
            Console.WriteLine("Data generation completed in {0}ms", stopWatch.ElapsedMilliseconds);
        }
    }
}
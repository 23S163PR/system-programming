using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace parallel_extension_demo
{


    class Program
    {
        static void Main(string[] args)
		{
			const int PeopleCount = 200000;
			var people = new List<Employee>(PeopleCount);
            ListGroups groups = new ListGroups();

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			Parallel.For(0, PeopleCount, i =>
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

				people.Add(employee);
			});


            groups.Colection.Add(new Group("group1.xml", people.Where(e => e.AgeInYears > 21 && e.Salary > 15000)
                .OrderBy(e => e.AgeInYears).ToList()));

            groups.Colection.Add(new Group("group2.xml", people.Where(e => e.Name.StartsWith("A") && e.Gender == Gender.Woman)
                .OrderBy(e => e.AgeInYears).ToList()));

            groups.Colection.Add(new Group("group3.xml", people.Where(e => e.Salary > 20000 && e.Gender == Gender.Men)
                .OrderBy(e => e.AgeInYears).ToList()));

            groups.Colection.Add(new Group("group4.xml", people.Where(e => e.AgeInYears > 50 && e.Gender == Gender.Transgender)
                .OrderBy(e => e.AgeInYears).ToList()));

			stopWatch.Stop();
			Console.WriteLine("Data generation completed in {0}ms", stopWatch.ElapsedMilliseconds);


            foreach (var item in groups.Colection)
            {
                Serializer.Serilizer(item, item.NameGroup);       
            }

          
		}
    }
}
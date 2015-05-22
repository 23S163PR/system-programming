using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_extension_demo
{
	

	class Program
	{
		static void Main(string[] args)
		{
			const int PeopleCount = 2000;
			var people = new List<Employee>(PeopleCount);

            var group1 = new List<Employee>();
            var group2 = new List<Employee>();
            var group3 = new List<Employee>();
            var group4 = new List<Employee>();


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

            group1.AddRange(people.Where(e => e.AgeInYears > 21 && e.Salary > 15000)
               .OrderBy(e => e.AgeInYears).ToList());

            group2.AddRange(people.Where(e => e.Name.StartsWith("A") && e.Gender == Gender.Woman)
             .OrderBy(e => e.AgeInYears).ToList());

            group3.AddRange(people.Where(e => e.Salary > 20000 && e.Gender == Gender.Men)
            .OrderBy(e => e.AgeInYears).ToList());

            group4.AddRange(people.Where(e => e.AgeInYears > 50 && e.Gender == Gender.Transgender)
            .OrderBy(e => e.AgeInYears).ToList());

			stopWatch.Stop();
			Console.WriteLine("Data generation completed in {0}ms", stopWatch.ElapsedMilliseconds);
		}
	}
}
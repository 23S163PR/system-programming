using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Employees.Faker.Extensions;
using Faker;

namespace Employees
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

			const int PeopleCount = 1000000;
			var peopleList = new List<Employee>(PeopleCount);

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			Parallel.For(0, PeopleCount, i =>
			{
				var name = Name.FullName();
				var employee = new Employee
				{
					Identity = Guid.NewGuid(),
					Name = name,
					Email = Internet.Email(name),
					AgeInYears = RandomNumber.Next(18, 80),
					Salary = RandomNumber.Next(10000, 30000), // $
					Gender = EnumExtensions.Rand<Gender>()
				};

				peopleList.Add(employee);
			});

			// 100 000 - 41055ms Phenom x3 2.7
			// 1 000 000 - 431803ms Phenom x3 2.7

			// 100 000 - 27489ms i5 3.1
			// 1 000 000 - 304793ms i5 3.1
			var firstFilterList =
				peopleList.AsParallel().Where(employee => employee.AgeInYears > 21 && employee.Salary > 15000)
					.ToList();
			var secondFilterList =
				peopleList.AsParallel().Where(employee => employee.Gender == Gender.Woman && employee.Name.StartsWith("A"))
					.ToList();
			var thirdFilterList =
				peopleList.AsParallel().Where(employee => employee.Gender == Gender.Men && employee.Salary > 20000)
					.ToList();
			var fourthFilterList =
				peopleList.AsParallel().Where(employee => employee.Gender == Gender.Transgender && employee.AgeInYears > 50)
					.ToList();

			stopWatch.Stop();

			Console.WriteLine("Generated and filtered in {0}ms", stopWatch.ElapsedMilliseconds);

			peopleList.SaveToJsonFile("employees.json");
			firstFilterList.SaveToJsonFile("first.json");
			secondFilterList.SaveToJsonFile("second.json");
			thirdFilterList.SaveToJsonFile("third.json");
			fourthFilterList.SaveToJsonFile("fourth.json");
		}
	}
}
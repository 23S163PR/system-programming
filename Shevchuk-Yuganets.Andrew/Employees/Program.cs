using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Employees.Faker.Extensions;
using Faker;

namespace Employees
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			const int PeopleCount = 10000;
			var peopleList = new List<Employee>(PeopleCount);
			var firstFilterList = new List<Employee>(PeopleCount);
			var secondFilterList = new List<Employee>(PeopleCount);
			var thirdFilterList = new List<Employee>(PeopleCount);
			var fourthFilterList = new List<Employee>(PeopleCount);


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
					Salary = RandomNumber.Next(10000, 30000),
					Gender = EnumExtensions.Rand<Gender>()
				};

				peopleList.Add(employee);
			});

			Parallel.ForEach(peopleList, employee =>
			{
				if (employee.AgeInYears > 21 && employee.Salary > 15000)
					firstFilterList.Add(employee);

				if (employee.Gender == Gender.Woman && employee.Name.StartsWith("A"))
					secondFilterList.Add(employee);

				if (employee.Gender == Gender.Men && employee.Salary > 20000)
					thirdFilterList.Add(employee);

				if (employee.Gender == Gender.Transgender && employee.AgeInYears > 50)
					fourthFilterList.Add(employee);
			});

			peopleList.SaveToJsonFile("employees.json");
			firstFilterList.SaveToJsonFile("first.json");
			secondFilterList.SaveToJsonFile("second.json");
			thirdFilterList.SaveToJsonFile("third.json");
			fourthFilterList.SaveToJsonFile("fourth.json");

			stopWatch.Stop();

			Console.WriteLine("Generated and filtered in {0}ms", stopWatch.ElapsedMilliseconds);

			Console.ReadLine();
		}
	}
}
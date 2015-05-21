using System;
using System.Collections.Generic;
using System.Diagnostics;
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

	class Program
	{
		static void Main(string[] args)
		{
			const int PeopleCount = 50000;
			var people = new List<Employee>(PeopleCount);

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

			stopWatch.Stop();
			Console.WriteLine("Data generation completed in {0}ms", stopWatch.ElapsedMilliseconds);
		}
	}
}
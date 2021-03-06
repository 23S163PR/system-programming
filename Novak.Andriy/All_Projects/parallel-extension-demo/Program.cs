﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faker;
using Faker.Extensions;

namespace parallel_extension_demo
{
    static class Program
	{
		static void Main(string[] args)
		{
			const int peopleCount = 20000;
		    var people = new List<Employee>(peopleCount);

		    var stopWatch = new Stopwatch();
			stopWatch.Start();
           
			Parallel.For(0, peopleCount, i =>
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

				people.Add(employee);
			});
		   var res = people.OrderBy(p=>p.AgeInYears);
           var groups = new Dictionary<string, Groups>();
           groups.Add("firstGroup.xml", new Groups(res.Where(p => p.AgeInYears > 21 && p.Salary > 15000)));
           groups.Add("secondGroup.xml", new Groups(res.Where(p => p.Gender == Gender.Woman && p.Name.StartsWith("A"))));
           groups.Add("thirdGroup.xml", new Groups(res.Where(p => p.Gender == Gender.Men && p.Salary > 20000)));
           groups.Add("fourthGroup.xml", new Groups(res.Where(p => p.Gender == Gender.Transgender && p.AgeInYears > 50)));

            foreach (var item in groups)
            {
                XSerializer<Groups>.XSerilizer(item.Value, item.Key);
            }
            
            stopWatch.Stop();
            using (var sw = new StreamWriter(Path.Combine(@"../../Groups Employee/", "log.txt"), true, Encoding.UTF8/*apends in end file*/))
            {
                foreach (var p in groups)
                {
                    sw.Write("File {0} \tContains - {1} records\n", p.Key,p.Value.Count);
                    sw.Flush();
                }
                sw.Write("Data generation completed in {0}ms\n", stopWatch.ElapsedMilliseconds);
            }
		}
	}
}
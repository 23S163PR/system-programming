using System;
using System.Linq;
using Faker;

namespace Employees.Faker.Extensions
{
	internal class EnumExtensions
	{
		public static T Rand<T>()
		{
			var values = Enum.GetValues(typeof (T)).Cast<T>().ToArray();
			return values[RandomNumber.Next(0, values.Length)];
		}
	}
}
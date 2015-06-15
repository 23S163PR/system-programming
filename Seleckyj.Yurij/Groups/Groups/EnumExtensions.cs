using System;
using System.Linq;

namespace Faker.Extensions
{
    public static class EnumExtensions
    {
        public static T Rand<T>()
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            return values[Faker.RandomNumber.Next(0, values.Length)];
        }
    }
}
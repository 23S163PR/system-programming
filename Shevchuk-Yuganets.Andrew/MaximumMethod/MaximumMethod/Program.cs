using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MaximumMethod
{
	class Program
	{
		static void Main(string[] args)
		{
			var arr = new List<int>();
			var rnd = new Random();
			for (var i = 0; i < 100; i++)
			{
				arr.Add(rnd.Next(100000));
			}

			var max = 0;
			Parallel.ForEach(arr, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, i =>
			{
				Maximum(ref max, i);
			});

			Console.WriteLine("max in arr - {0}", arr.Max());
			Console.WriteLine("max in Maximum method - {0}", max);
		}

		static void Maximum(ref int oldValue, int newValue)
		{
			if (Volatile.Read(ref oldValue) < Volatile.Read(ref newValue))
			{
				// Interlocked.Exchange(ref oldValue, newValue);
				Volatile.Write(ref oldValue, newValue);
			}
		}
	}
}

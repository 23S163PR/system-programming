using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_extensions
{
	class Program
	{
		static void Main(string[] args)
		{
			Parallel.For(0, 200000, new ParallelOptions { MaxDegreeOfParallelism = 2 }, i =>
			{
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
			});

			Parallel.ForEach(Enumerable.Range(0, 1000), i =>
			{
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
			});

			Enumerable.Range(0, 1000)
				.AsParallel()
				.ForAll(i =>
				{
					Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
				});
		}
	}
}

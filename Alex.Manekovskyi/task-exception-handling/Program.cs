using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task_exception_handling
{
	class Program
	{
		static void Main(string[] args)
		{
			// #1 Exception is not handled
			var sumTask = Task.Factory.StartNew(Sum, 500);
			while (!sumTask.IsCompleted) // We are intentionally not triggering exception
			{
				Thread.Sleep(250 /* milliseconds */);
			}

			if (sumTask.IsFaulted)
			{
				PrintOutExceptionMessages(sumTask.Exception);
			}
			sumTask.Dispose();

			// #2 Exception is handled in try-catch block
			sumTask = Task.Factory.StartNew(Sum, 500);
			try
			{
				sumTask.Wait();
			}
			catch (AggregateException e)
			{
				PrintOutExceptionMessages(e);
			}
			finally
			{
				sumTask.Dispose();
			}

			// #3 Exception is handled using AggregateException.Handle method
			sumTask = Task.Factory.StartNew(Sum, 500);
			try
			{
				sumTask.Wait();
			}
			catch (AggregateException e)
			{
				Console.WriteLine("Following exceptions occured:\n");
                e.Handle(exception => 
				{
					Console.WriteLine(exception.Message);
                    return true;
                });
			}
			finally
			{
				sumTask.Dispose();
			}
		}

		private static int Sum(object state)
		{
			var count = (int)state;
			if (count > 100)
			{
				throw new InvalidOperationException("Only values that are less than 100 are allowed.");
			}

			var sum = 0;
			for (int i = 0; i < count; i++)
			{
				checked { sum += i; }
			}

			return sum;
		}

		private static void PrintOutExceptionMessages(AggregateException exception)
		{
			var exceptions = exception.Flatten().InnerExceptions;
			Console.WriteLine(
				"Following exceptions occured:\n{0}", 
				string.Join("\n", exceptions.Select(e => e.Message)));
		}
	}
}

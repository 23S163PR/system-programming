using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task_cancellation
{
	class Program
	{
		static void Main(string[] args)
		{
			var cts = new CancellationTokenSource();
			var token = cts.Token;

			var sumTask = Task.Factory.StartNew(() => Sum(4000, token), token);
			Console.WriteLine("sumTask.isCancelled = {0}", sumTask.IsCanceled);

			cts.Cancel();

			try
			{
				Console.WriteLine("Sum(4000) = {0}", sumTask.Result);
			}
			catch (AggregateException e)
			{
				PrintOutExceptionMessages(e);
			}
		}

		private static int Sum(int count, CancellationToken token)
		{
			var sum = 0;
			for (int i = 0; i < count; i++)
			{
				token.ThrowIfCancellationRequested();

				sum += i;
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

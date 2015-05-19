using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace threads_creation
{
	class Program
	{
		static void Main(string[] args)
		{
			var cancellationtokenSource = new CancellationTokenSource();
			var cancelationToken = cancellationtokenSource.Token;

			var doer = new Thread(() => {
				DoSleep(1000, cancelationToken);
			});

			doer.Start();

			Console.WriteLine("Press Enter key to stop DoSleep...");
			Console.ReadLine();

			cancellationtokenSource.Cancel();

			Console.WriteLine("DoSleep stop initiated");

			Console.WriteLine("Press Enter again to exit...");
			Console.ReadLine();
		}

		static void DoSleep(int iterationCount, CancellationToken token)
		{
			while (iterationCount-- > 0)
			{
				if (token.IsCancellationRequested) break;

				Thread.Sleep(1000); // 1 sec
			}

			Console.WriteLine("Operation cancelled. {0} iterations left.", iterationCount);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cancellation_token_source
{
	class Program
	{
		static void Main(string[] args)
		{
			// Linked cancellation token sources
			Console.WriteLine("EXAMPLE 1: Linked CancellationTokenSource");

			var cts1 = new CancellationTokenSource();
			cts1.Token.Register(() => Console.WriteLine("cts1 cancelled"));

			var cts2 = new CancellationTokenSource();
			cts2.Token.Register(() => Console.WriteLine("cts2 cancelled"));

			var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);

			cts2.Cancel();

			Console.WriteLine("cts1.IsCancellationRequested={0}\ncts2.IsCancellationRequested={1}\nlinkedCts.IsCancellationRequested={2}",
				cts1.IsCancellationRequested, cts2.IsCancellationRequested, linkedCts.IsCancellationRequested);

			Console.WriteLine("Press Enter to continue");
			Console.ReadLine();

			// Timeout
			Console.WriteLine("EXAMPLE 2: CancellationTokenSource timeout");

			var timeoutEnabledCTS = new CancellationTokenSource(1000 /* 1 sec */);
			Console.WriteLine("Before 2 sec sleep: timeoutEnabledCTS.IsCancellationRequested = {0}", timeoutEnabledCTS.IsCancellationRequested);
			Thread.Sleep(2000);
			Console.WriteLine("After 2 sec sleep: timeoutEnabledCTS.IsCancellationRequested = {0}", timeoutEnabledCTS.IsCancellationRequested);
		}
	}
}

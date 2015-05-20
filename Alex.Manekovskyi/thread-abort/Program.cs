using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace thread_abort
{
	class Program
	{
		static void Main(string[] args)
		{
			var thread = new Thread(CPUBoundOperation);
			thread.Start();

			Console.WriteLine("Press Enter to make attempt #1 to abort thread");
			Console.ReadLine();
			thread.Abort();

			Console.WriteLine("Press Enter to make attempt #2 to abort thread");
			Console.ReadLine();
			thread.Abort();

			Console.WriteLine("Press Enter to exit program...");
			Console.ReadLine();
		}

		private static void CPUBoundOperation(object obj)
		{
			var abortAttemptsCount = 0;
			while (abortAttemptsCount < 2)
			{
				try
				{
					while (true)
					{
						Thread.Sleep(1000 /* 1 sec */);
					}
				}
				catch (ThreadAbortException e)
				{
					abortAttemptsCount++;
					if (abortAttemptsCount < 2)
					{
						Console.WriteLine("THREAD: I see you want to stop me. But I'm not ready to stop yet!");
					}
					Thread.ResetAbort();
				}
			}

			Console.WriteLine("THREAD: Ok, now it is time for me to stop.");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace autoresetevent_performance_demo
{
	class Program
	{
		static void Main(string[] args)
		{
			int x = 0;
			const int IterationsCount = 10000000;

			// #1 - simple increments
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < IterationsCount; i++)
			{
				x++;
			}
			Console.WriteLine("Incrementing x: {0:N0}", stopwatch.ElapsedMilliseconds);

            // #2 - increments surrounded by method
            stopwatch.Restart();
            for (int i = 0; i < IterationsCount; i++)
            {
                M();
                x++;
                M();
            }
            Console.WriteLine("Incrementing x surrounded by M: {0:N0}", stopwatch.ElapsedMilliseconds);

            // #3 - increments with non blocking SpinLock
            var spinLock = new SpinLock(enableThreadOwnerTracking: false);
            stopwatch.Restart();
            for (int i = 0; i < IterationsCount; i++)
            {
                var taken = false;
                spinLock.Enter(ref taken);
                x++;
                spinLock.Exit();                
            }
            Console.WriteLine("Incrementing x in SpinLock: {0:N0}", stopwatch.ElapsedMilliseconds);

            // #4 - increments with AutoResetEvent
            using (var @event = new AutoResetEvent(initialState: true))
            {
                stopwatch.Restart();
                for (int i = 0; i < IterationsCount; i++)
                {
                    @event.WaitOne();
                    x++;
                    @event.Set();
                }
                Console.WriteLine("Incrementing x in AutoResetEvent: {0:N0}", stopwatch.ElapsedMilliseconds);
            }
        }

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void M() { /* does nothing */ }
	}
}

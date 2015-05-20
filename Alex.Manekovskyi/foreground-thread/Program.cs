using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace foreground_thread
{
	class Program
	{
		static void Main(string[] args)
		{
			// The process cannot be stopped until all foreground threads are not stopped
			var thread = new Thread(() =>
			{
				Thread.Sleep(5000 /* 5 sec */);
			});
			thread.IsBackground = false;
			thread.Start();
		}
	}
}

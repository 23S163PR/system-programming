using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace volatile_demo
{
	// Example code taken as-is from CLR via C# book
	class StrageBehavior
	{
		private static bool s_stopWorker = false;

		static void Main(string[] args)
		{
			Console.WriteLine("Main: letting worker run for 5 seconds");
			var t = new Thread(Worker);
			t.Start();
			Thread.Sleep(5000 /* 5 sec */);
			s_stopWorker = true;
			Console.WriteLine("Main: waiting for worker to stop");
			t.Join();
		}

		private static void Worker()
		{
			int x = 0;
			while (!s_stopWorker) x++;

			Console.WriteLine("Worker: stopped when x={0}", x);
		}
	}

	class ThreadsSharingData
	{
		private int m_flag = 0;
		private int m_value = 0;

		public void Thread1()
		{
			m_value = 5;
			m_flag = 1;
		}

		public void Thread2()
		{
			if (m_flag == 1)
			{
				Console.WriteLine(m_value);
			}
		}
	}
}

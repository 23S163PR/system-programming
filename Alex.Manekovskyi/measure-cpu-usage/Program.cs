using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace measure_cpu_usage
{
	/// <summary>
	/// This sample shows how to measure the CPU utilization
	/// by each process currently running.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			var processes = Process.GetProcesses();
			var performanceCounters = new List<PerformanceCounter>();
			foreach (var process in processes)
			{
				performanceCounters.Add(new PerformanceCounter("Process", "% Processor Time", process.ProcessName));
			}

			while (true)
			{
				Thread.Sleep(1000); // 1000ms = 1s
				Console.SetCursorPosition(0, 0);
				for (int processIndex = 0; processIndex < processes.Length; processIndex++)
				{
					Console.WriteLine("{0,-60}{1,-5}", processes[processIndex].ProcessName, performanceCounters[processIndex].NextValue().ToString("F2"));
				}
			}
		}
	}
}

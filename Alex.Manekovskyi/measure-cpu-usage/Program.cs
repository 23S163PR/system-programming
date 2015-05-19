using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
			var performanceCountersByProcessId = new Dictionary<int, PerformanceCounter>();
			foreach (var process in processes)
			{
				var counter = process.GetPerformanceCounter("% Processor Time");
				if (counter == null) continue;

				performanceCountersByProcessId.Add(process.Id, counter);
			}

			while (true)
			{
				Thread.Sleep(1000); // 1000ms = 1s
				Console.SetCursorPosition(0, 0);
				for (int processIndex = 0; processIndex < processes.Length; processIndex++)
				{
					var process = processes[processIndex];
					Console.WriteLine("{0,-60}{1,-5}",
						process.ProcessName,
						performanceCountersByProcessId[process.Id].NextValue().ToString("F2"));
				}
			}
		}
	}

	public static class ProcessExtensions
	{
		/// <summary>
		/// Returns specified performance counter associated wiht the process or null if there was no match.
		///
		/// <remarks>
		/// The idea is taken from http://weblog.west-wind.com/posts/2014/Sep/27/Capturing-Performance-Counter-Data-for-a-Process-by-Process-Id.
		/// See also "The Process object in Performance Monitor
		/// can display Process IDs (PIDs)" MSDN article: https://support.microsoft.com/en-us/kb/281884.
		/// </remarks>
		/// </summary>
		/// <param name="process">The process instance.</param>
		/// <param name="processCounterName">The process counter name.</param>
		/// <returns>Performance counter associated with the process.</returns>
		public static PerformanceCounter GetPerformanceCounter(this Process process, string processCounterName)
		{
			var processName = Path.GetFileNameWithoutExtension(process.ProcessName);

			var processCategory = new PerformanceCounterCategory("Process");
			var similarInstances = processCategory.GetInstanceNames()
				.Where(instance => instance.StartsWith(processName));

			foreach (var instance in similarInstances)
			{
				using (var processIdCounter = new PerformanceCounter("Process", "ID Process", instance, true /* readOnly */))
				{
					int instanceId = (int)processIdCounter.RawValue;
					if (instanceId == process.Id)
					{
						return new PerformanceCounter("Process", processCounterName, instance);
					}
				}
			}

			return null;
		}
	}
}

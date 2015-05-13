using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using TaskManager.Model;

namespace TaskManager.Control
{
	public static class MainControl
	{
		public static List<SystemProcess> GetProcessList()
		{
			var processes = Process.GetProcesses();

			return processes.Select(process => new SystemProcess
			{
				Name = process.ProcessName,
				Threads = process.Threads.Count,
				Memory = GetMemory(process),
				Cpu = GetCpu(process),
				ProcessId = process.Id
			}).ToList();
		}

		private static float GetMemory(Process process)
		{
			return process.WorkingSet64/1024f;
		}

		private static float GetCpu(Process process)
		{
			// var performanceCounters = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
			// return new PerformanceCounter("Process", "% Processor Time", process.ProcessName).NextValue();

			return 0;
		}

		private static Process GetProcess(int id)
		{
			return Process.GetProcessById(id);
		}

		public static ProcessPriorityClass GetProcessPriority(int id)
		{
			return GetProcess(id).PriorityClass;
		}

		public static void SetProcessPriority(int id, ProcessPriorityClass priority)
		{
			GetProcess(id).PriorityClass = priority;
		}

		public static void KillProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
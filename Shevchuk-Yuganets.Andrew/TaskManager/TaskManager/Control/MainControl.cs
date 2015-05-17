using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaskManager.Model;

namespace TaskManager.Control
{
	public static class MainControl
	{
		public static List<ProcessModel> GetProcessList()
		{
			var processes = Process.GetProcesses();

			return (from process in processes
					where process.ProcessName != "Idle"
					select new ProcessModel
					{
						ProcessId = process.Id,
						Name = GetProcessName(process),
						Threads = GetThreads(process),
						MemoryUsage = GetMemoryUsage(process),
						CpuUsage = GetCpuUsage(process)
					}).ToList();
		}

		private static string GetProcessName(Process process)
		{
			return process.ProcessName + ".exe";
		}

		private static string GetThreads(Process process)
		{
			return process.Threads.Count.ToString();
		}

		private static string GetMemoryUsage(Process process)
		{
			return ((process.WorkingSet64 / 1024F) / 1024F).ToString("F3") + " K";
		}

		private static string GetCpuUsage(Process process)
		{
			// TODO: need solution how to get process cpu usage
			return "none";
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
			try
			{
				GetProcess(id).PriorityClass = priority;
			}
			catch (Exception)
			{
				// ignored
			}
		}

		public static void KillProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
			}
			catch (Exception)
			{
				// ignored
			}
		}
	}
}
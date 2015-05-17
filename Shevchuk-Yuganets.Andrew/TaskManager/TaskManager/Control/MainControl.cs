using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using TaskManager.Model;

namespace TaskManager.Control
{
	public static class MainControl
	{
		public static List<ProcessModel> GetProcessList()
		{
			var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");

			return (searcher.Get()
				.Cast<ManagementObject>()
				.Where(obj => ConvertObjToName(obj["Name"]) != "Idle")
				.Where(obj => ConvertObjToName(obj["Name"]) != "_Total")
				.Select(obj => new ProcessModel
				{
					ProcessId = ConvertObjToId(obj["IDProcess"]),
					Name = ConvertObjToName(obj["Name"]),
					Threads = ConvertObjToThreads(obj["ThreadCount"]),
					CpuUsage = ConvertObjToCpuUsage(obj["PercentProcessorTime"]),
					MemoryUsage = ConvertObjToMemoryUsage(obj["WorkingSet"])
				})).ToList();
		}

		private static int ConvertObjToId(object obj)
		{
			return int.Parse(obj.ToString());
		}

		private static string ConvertObjToName(object obj)
		{
			return obj.ToString();
		}

		private static string ConvertObjToThreads(object obj)
		{
			return obj.ToString();
		}

		private static string ConvertObjToCpuUsage(object obj)
		{
			return obj.ToString();
		}

		private static string ConvertObjToMemoryUsage(object obj)
		{
			return ((int.Parse(obj.ToString()) / 1024F) / 1024F).ToString("F3") + " K";
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
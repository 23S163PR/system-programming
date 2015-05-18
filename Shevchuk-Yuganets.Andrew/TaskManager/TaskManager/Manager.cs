using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows;

namespace TaskManager
{
	public static class Manager
	{
		public static List<ProcessModel> GetProcessList()
		{
			var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");

			return (searcher.Get()
				.Cast<ManagementObject>()
				.Where(obj => obj["Name"].ToString() != "Idle")
				.Where(obj => obj["Name"].ToString() != "_Total")
				.Select(obj => new ProcessModel
				{
					ProcessId = int.Parse(obj["IDProcess"].ToString()),
					Name = obj["Name"].ToString(),
					Threads = obj["ThreadCount"].ToString(),
					CpuUsage = string.Format("{0} %", obj["PercentProcessorTime"]),
					MemoryUsage = string.Format("{0:f} K", (int.Parse(obj["WorkingSet"].ToString()) / 1024F) / 1024F)
				})).ToList();
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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
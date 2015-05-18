using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace TaskManager
{
	public static class ProcessManager
	{
		public static List<ProcessModel> GetProcessList()
		{
			return WmiManager.GetProcessList();
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
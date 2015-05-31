using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace TaskManager
{
	public class ProcessManager
	{
		private ObservableCollection<ProcessModel> _processList;

		public ObservableCollection<ProcessModel> ProcessList
		{
			get
			{
				return _processList;
			}
		}

		public ProcessManager()
		{
			_processList = WmiManager.GetProcessList();
		}

		public void UpdateList()
		{
			var dispather = Application.Current.Dispatcher;
			var wmiProcessList = WmiManager.GetProcessList();

			foreach (var process in wmiProcessList)
			{
				var tmpProcess = _processList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess != null)
				{
					dispather.Invoke(() => {
						tmpProcess.CpuUsage = process.CpuUsage;
						tmpProcess.MemoryUsage = process.MemoryUsage;
						tmpProcess.Threads = process.Threads;
					});
				}
				else
				{
					dispather.Invoke(() => _processList.Add(process));
				}
			}

			var closedProcess = new List<int>();
			foreach (var process in _processList)
			{
				var tmpProcess = wmiProcessList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess == null)
				{
					closedProcess.Add(process.ProcessId);
				}
			}

			foreach (var processId in closedProcess)
			{
				dispather.Invoke(() => _processList.Remove(_processList.FirstOrDefault(pr => pr.ProcessId == processId)));
			}
		}

		private Process GetProcess(int id)
		{
			return Process.GetProcessById(id);
		}

		public ProcessPriorityClass GetProcessPriority(int id)
		{
			return GetProcess(id).PriorityClass;
		}

		public void SetProcessPriority(int id, ProcessPriorityClass priority)
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

		public void KillProcess(int id)
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
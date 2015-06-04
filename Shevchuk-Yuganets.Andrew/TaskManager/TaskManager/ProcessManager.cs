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
		private readonly ObservableCollection<ProcessModel> _processList;

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
			// TODO: dispatcher must be in MainWindow.xaml.cs - UI
			// TODO: need implement events for "add", "delete", "update"
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
				ExceptionHandler.HandleError(OperationType.ChangeProcessPriorityClass, ex);
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

	public enum OperationType
	{
		ChangeProcessPriorityClass
	}

	// TODO: need special exceptions for "GetProcess", "GetProcessPriority", "SetProcessPriority"
	public class ExceptionHandler
	{
		public static void HandleError(OperationType operation, Exception exception)
		{
			switch (operation)
			{
				case OperationType.ChangeProcessPriorityClass:
					if (exception is ArgumentException)
					{
						MessageBox.Show(exception.Message);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
			}
		}
	}
}
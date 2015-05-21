using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace TaskManager
{
	public class ProcessManager
	{
		private DispatcherTimer _dispatcherTimer;
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

			_dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 100) };
			_dispatcherTimer.Tick += OneTime_Tick;
		}

		public void UpdateList()
		{
			// TODO: need idea how to update data using DispatherTimer without scrollbar freeze
			// _dispatcherTimer.Start();
			// UpdateListTest();
		}

		void OneTime_Tick(object sender, EventArgs e)
		{
			var wmiProcessList = WmiManager.GetProcessList();

			foreach (var process in wmiProcessList)
			{
				var tmpProcess = _processList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess != null)
				{
					tmpProcess.CpuUsage = process.CpuUsage;
					tmpProcess.MemoryUsage = process.MemoryUsage;
					tmpProcess.Threads = process.Threads;
				}
				else
				{
					_processList.Add(process);
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
				_processList.Remove(_processList.FirstOrDefault(pr => pr.ProcessId == processId));
			}

			_dispatcherTimer.Stop();
        }

		private void UpdateListTest()
		{
			var wmiProcessList = WmiManager.GetProcessList();

			foreach (var process in wmiProcessList)
			{
				var tmpProcess = _processList.FirstOrDefault(pr => pr.ProcessId == process.ProcessId);
				if (tmpProcess != null)
				{
					tmpProcess.CpuUsage = process.CpuUsage;
					tmpProcess.MemoryUsage = process.MemoryUsage;
					tmpProcess.Threads = process.Threads;
				}
				else
				{
					_processList.Add(process);
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
				_processList.Remove(_processList.FirstOrDefault(pr => pr.ProcessId == processId));
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace taskmsg
{
	class Taskmsg
	{
		private readonly ObservableCollection<ProcessModel> _procs;

		public int CurentProcessId { get; set; }

		public Taskmsg()
		{
			_procs = new ObservableCollection<ProcessModel>();
			RefreshProcesses();
		}

		public ObservableCollection<ProcessModel> Processes
		{
			get
			{
				return _procs;
			}
		}

		private static TimeSpan GetProcessTime(int id)
		{
			var time = new TimeSpan();
			try
			{
				time = Process.GetProcessById(id).TotalProcessorTime;
			}
			catch (Exception)
			{
				//if we do not have access to the system process
				return time;
			}
			return time; 
		}

		public void RefreshProcesses()
		{
			var res = GetCUrentProcesses();
			if (!_procs.Any())
			{
				foreach (var p in res)
				{
					_procs.Add(p);
				}
			}
			else
			{
				foreach (var item in res)
				{
					var curent = _procs.FirstOrDefault(p => p.Id == item.Id);
					if (curent != null)
					{
						ProcessModel.CompareChanger(ref curent, item);
					}
					else
					{
						_procs.Add(item);
					}
				}
			}
		}

		private static IEnumerable<ProcessModel> GetCUrentProcesses()
		{
			var proc = Process.GetProcesses();
			var res = proc.Where(t => t.ProcessName != "Idle").Select(p =>
				new ProcessModel
					(
					p.Id
					, p.ProcessName
					, (p.WorkingSet64/1024f)/1024f
					, p.Threads.Count
					, GetProcessTime(p.Id)
					, PersentProcessorTime(GetProcessTime(p.Id))
					))
				.OrderBy(a => a.Name).ThenBy(i => i.Id).ToList();
			return res;
		}
		//(увеличение CPU time за минуту)/(1 минута)*100% = средняя загрузка CPU процессом за последнюю минуту.
		private static string PersentProcessorTime(TimeSpan time)
		{
			
			var res = (int)(time.TotalMilliseconds / (1000)) * 100;
			return string.Format("{0} %", res < 0 ? 0 : res);
		}

		private static Process GetProcess(int id)
		{
			return Process.GetProcessById(id);
		}

		public ProcessPriorityClass GetProcesPriorityClass(int id)
		{
			return GetProcess(id).PriorityClass;
		}

		public static void SetProcessPrioruty(int id, ProcessPriorityClass priority)
		{
			try
			{
				GetProcess(id).PriorityClass = priority;
			}
			catch (Exception){}
		}

		public void CloseProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
				var item = _procs.FirstOrDefault(p => p.Id == id);
				_procs.Remove(item);
			}
			catch(Exception){}
		}
	}
}

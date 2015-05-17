using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using taskmsg.Annotations;

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

		public IEnumerable<ProcessModel> Processes
		{
			get
			{
				return _procs;
			}
		}

		public void RefreshProcesses()
		{
			if (_procs.Any())
			{
				_procs.Clear();
			}
			var proc = Process.GetProcesses();
			var res = proc.Where(t => t.ProcessName != "Idle").Select(p => 
				new ProcessModel
				(
				p.Id
				, p.ProcessName
				, (p.WorkingSet64 / 1024f) / 1024f
				, p.Threads.Count
				//, p.UserProcessorTime
				,new TimeSpan()
				))
				.OrderBy(a => a.Name).ThenBy(i => i.Id);
			foreach (var pr in res)
			{
				_procs.Add(pr);
			}
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

		public static void CloseProcess(int id)
		{
			try
			{
				GetProcess(id).Kill();
			}
			catch(Exception){}
		}
	}
}

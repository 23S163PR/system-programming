using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace taskmsg
{
	class Taskmsg
	{
		private  ObservableCollection<ProcessModel> _procs;

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

		[DllImport("kernel32.dll")]
		static extern bool GetProcessTimes(IntPtr hProcess,
			out FILETIME lpCreationTime,
			out FILETIME lpExitTime,
			out FILETIME lpKernelTime,
			out FILETIME lpUserTime);
		
		
		public static TimeSpan FiletimeToTimeSpan(FILETIME fileTime)
		{
			var hFT2 = unchecked((((ulong)(uint)fileTime.dwHighDateTime) << 32) | (uint)fileTime.dwLowDateTime);
			return TimeSpan.FromTicks((long)hFT2);
		}

		//(увеличение CPU time за минуту)/(1 минута)*100% = средняя загрузка CPU процессом за последнюю минуту.
		private static TimeSpan GetProcessTime(int id, ref int cpu)
		{
			FILETIME ftCreation, ftExit, ftKernel, ftUser;
			try
			{
				var ip = Process.GetProcessById(id).Handle;
				GetProcessTimes(ip, out ftCreation, out ftExit, out ftKernel, out ftUser);
			}
			catch (Exception)
			{
				cpu = 0;
				return new TimeSpan();
			}
			cpu = (int)(FiletimeToTimeSpan(ftUser).TotalMilliseconds / (1000 * 60)) * 100;
			return FiletimeToTimeSpan(ftUser);
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


		private static List<ProcessModel> GetCUrentProcesses()
		{
			var proc = Process.GetProcesses();
			var cputime = 0;
			var res = proc.Where(t => t.ProcessName != "Idle").Select(p =>
				new ProcessModel
					(
					p.Id
					, p.ProcessName
					, (p.WorkingSet64/1024f)/1024f
					, p.Threads.Count
					, GetProcessTime(p.Id, ref cputime)
					, cputime
					))
				.OrderBy(a => a.Name).ThenBy(i => i.Id).ToList();
			return res;
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

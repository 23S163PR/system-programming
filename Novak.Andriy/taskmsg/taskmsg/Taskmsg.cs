using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

		public static string GetProcessTime(int id)
		{
			var time = new TimeSpan();
			try
			{
				time = Process.GetProcessById(id).TotalProcessorTime;
			}
			catch (Exception)
			{
				//if we do not have access to the system process
                return time.ToString(@"hh\:mm\:ss"); 
			}
            return time.ToString(@"hh\:mm\:ss"); 
		}

	    private void ClearOld(IEnumerable<Process> processes)
	    {
            var id = processes.Select(t => t.Id);
	        var dels =  _procs.Where(p => !id.Contains(p.Id)).ToList();
	        if (!dels.Any()) return;
	        foreach (var item in dels)
	        {
	            _procs.Remove(item);
	        }
	    }

		public void RefreshProcesses()
		{
            var proc = Process.GetProcesses();
            var res = proc.Where(t => t.ProcessName != "Idle").OrderBy(p=>p.ProcessName).ThenBy(p=>p.Id);

		    ClearOld(res);
            foreach (var p in res)
            {
                var curent = _procs.FirstOrDefault(c => c.Id == p.Id);
                if (curent != null)
                {
                    curent = ProcessModel.CompareChanger(curent, p);
                }
                else
                {
                    var process = new ProcessModel
                        (
                        p.Id
                        , p.ProcessName
                        , (p.WorkingSet64 / 1024f) / 1024f
                        , p.Threads.Count
                        , GetProcessTime(p.Id)
                        //, PersentProcessorTime(GetProcessTime(p.Id))
                        );
                    _procs.Add(process);
                }
                
            }
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

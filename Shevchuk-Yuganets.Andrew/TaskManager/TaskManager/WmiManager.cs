using System.Collections.ObjectModel;
using System.Linq;
using System.Management;

namespace TaskManager
{
	public class WmiManager
	{
		public static ObservableCollection<ProcessModel> GetProcessList()
		{
			var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfProc_Process");

			var collection = new ObservableCollection<ProcessModel>();

			foreach (var obj in searcher.Get()
				.Cast<ManagementObject>()
				.Where(obj => obj["Name"].ToString() != "Idle")
				.Where(obj => obj["Name"].ToString() != "_Total"))
			{
				collection.Add(new ProcessModel
				{
					ProcessId = int.Parse(obj["IDProcess"].ToString()),
					Name = obj["Name"].ToString(),
					Threads = obj["ThreadCount"].ToString(),
					CpuUsage = $"{obj["PercentProcessorTime"]} %",
					MemoryUsage = $"{(int.Parse(obj["WorkingSet"].ToString())/1024F)/1024F:f} K"
				});
			}
			return collection;
		}
	}
}

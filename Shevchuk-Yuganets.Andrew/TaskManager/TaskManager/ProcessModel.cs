namespace TaskManager
{
	public class ProcessModel
	{
		public int ProcessId { set; get; }
		public string Name { set; get; }
		public string Threads { set; get; }
		public string MemoryUsage { set; get; }
		public string CpuUsage { set; get; }
	}
}
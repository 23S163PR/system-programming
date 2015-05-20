using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskManager
{
	public class ProcessModel : INotifyPropertyChanged
	{
		private string _threads;
		private string _memoryUsage;
		private string _cpuUsage;

		public int ProcessId { get; set; }

		public string Name{ get; set; }

		public string Threads
		{
			get { return _threads; }
			set
			{
				if (_threads == value)
					return;

				_threads = value;
				InvokePropertyChanged();
			}
		}

		public string MemoryUsage
		{
			get { return _memoryUsage; }
			set
			{
				if (_memoryUsage == value)
					return;

				_memoryUsage = value;
				InvokePropertyChanged();
			}
		}

		public string CpuUsage
		{
			get { return _cpuUsage; }
			set
			{
				if (_cpuUsage == value)
					return;

				_cpuUsage = value;
				InvokePropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void InvokePropertyChanged([CallerMemberName] string member = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(member));
			}
		}
	}
}
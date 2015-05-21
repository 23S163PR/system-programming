using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskManager
{
	public class ProcessModel : INotifyPropertyChanged
	{
		private int _processId;
		private string _name;
		private string _threads;
		private string _memoryUsage;
		private string _cpuUsage;

		public int ProcessId
		{
			get { return _processId; }
			set
			{
				if (_processId == value)
					return;

				_processId = value;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
					return;

				_name = value;
				OnPropertyChanged();
			}
		}

		public string Threads
		{
			get { return _threads; }
			set
			{
				if (_threads == value)
					return;

				_threads = value;
				OnPropertyChanged();
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
				OnPropertyChanged();
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
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string member = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(member));
			}
		}
	}
}
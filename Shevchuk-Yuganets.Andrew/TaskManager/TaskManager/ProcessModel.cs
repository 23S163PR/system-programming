using System.Collections.Generic;
using System.ComponentModel;

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
				Set(ref _processId, value, nameof(ProcessId));
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				Set(ref _name, value, nameof(Name));
			}
		}

		public string Threads
		{
			get { return _threads; }
			set
			{
				Set(ref _threads, value, nameof(Threads));
			}
		}

		public string MemoryUsage
		{
			get { return _memoryUsage; }
			set
			{
				Set(ref _memoryUsage, value, nameof(MemoryUsage));
			}
		}

		public string CpuUsage
		{
			get { return _cpuUsage; }
			set
			{
				Set(ref _cpuUsage, value, nameof(CpuUsage));
			}
		}

		private void Set<T>(ref T field, T newValue, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, newValue)) return;

			field = newValue;
			OnPropertyChanged(propertyName);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string member = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(member));
			}
		}
	}
}
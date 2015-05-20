using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using taskmsg.Annotations;

namespace taskmsg
{
	public sealed class ProcessModel : INotifyPropertyChanged
	{
		private int		_id;
		private string  _name;
		private double	_memory;
		private int		_treads;
		private string	_cpuTime;   
		private string	_cpuTimePersent;

		#region Properties
		public int Id
		{
			private set
			{
				if (_id == value) return;
				_id = value;
				ProrertyChange();
			}
			get { return _id; }
		}

		public string Name
		{
			private set
			{
				if (_name == value) return;
				_name = value;
				ProrertyChange();
			}
			get { return _name; }
		}

		public double Memory
		{
			private set
			{
				if (_memory == value) return;
				_memory = value;
				ProrertyChange();
			}
			get { return _memory; }
		}

		public int Treads
		{
			private set
			{
				if (_treads == value) return;
				_treads = value;
				ProrertyChange();
			}
			get { return _treads; }
		}

		public string CpuTime
		{
			private set
			{
				if (_cpuTime == value) return;
				_cpuTime = value;
				ProrertyChange();
			}
			get { return _cpuTime; }
		}

		public string CpuTimePersent
		{
			private set
			{
				if (_cpuTimePersent == value) return;
				_cpuTimePersent = value;
				ProrertyChange();
			}
			get { return _cpuTimePersent; }
		}
		#endregion

        public ProcessModel(int id, string name, double memory, int treads, string ms/*, string cputime*/)
		{
			Id = id;
			Name = name;
			Memory = Math.Round(memory, 3);
			Treads = treads;
		    CpuTime = ms;
			//CpuTimePersent = cputime;
		}

        public static ProcessModel CompareChanger(ProcessModel dest, Process sourse)
		{
			dest.Id	= sourse.Id;
            dest.Name = sourse.ProcessName;
		    dest.Memory =  Math.Round((sourse.WorkingSet64/1024f)/1024f,3);
            dest.Treads = sourse.Threads.Count;
            dest.CpuTime = Taskmsg.GetProcessTime(sourse.Id);
			//dest.CpuTimePersent 
            return dest;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ProrertyChange([CallerMemberName] string member = null)
		{
			if(PropertyChanged == null) return;
			PropertyChanged(this, new PropertyChangedEventArgs(member));
		}


	}
}

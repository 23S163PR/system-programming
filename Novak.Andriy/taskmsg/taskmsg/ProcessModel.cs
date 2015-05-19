using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using taskmsg.Annotations;

namespace taskmsg
{
	public class ProcessModel : INotifyPropertyChanged
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

		public ProcessModel(int id, string name, double memory, int treads, TimeSpan ms, int cputime)
		{
			Id = id;
			Name = name;
			Memory = Math.Round(memory, 3);
			Treads = treads;
			CpuTime = ms.ToString(@"hh\:mm\:ss");
			CpuTimePersent = string.Format("{0}%", cputime);
		}

		public static void CompareChanger(ref ProcessModel dest, ProcessModel sourse)
		{
			dest.Id				= sourse.Id;
			dest.Name			= sourse.Name;
			dest.Memory			= sourse.Memory;
			dest.Treads			= sourse.Treads;
			dest.CpuTime		= sourse.CpuTime;
			dest.CpuTimePersent = sourse.CpuTimePersent;	
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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

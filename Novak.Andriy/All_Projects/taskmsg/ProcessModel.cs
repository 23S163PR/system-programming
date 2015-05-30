using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;


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
                Set(ref _id, value, () => Id);
			}
			get { return _id; }
		}

		public string Name
		{
			private set
			{
                Set(ref _name, value, () => Name);
			}
			get { return _name; }
		}

		public double Memory
		{
			private set
			{
                Set(ref _memory, value, () => Memory);
			}
			get { return _memory; }
		}

		public int Treads
		{
			private set
			{
                Set(ref _treads, value, ()=> Treads);
			}
			get { return _treads; }
		}

		public string CpuTime
		{
			private set
			{
                Set(ref _cpuTime, value, () => CpuTime);
			}
		    get { return _cpuTime; }
		}

		public string CpuTimePersent
		{
			private set
			{
                Set(ref _cpuTimePersent, value, () => CpuTimePersent);
			}
			get { return _cpuTimePersent; }
		}
		#endregion

        public ProcessModel(int id, string name, double memory, int treads, string ms, string cputime)
		{
			Id = id;
			Name = name;
			Memory = Math.Round(memory, 3);
			Treads = treads;
		    CpuTime = ms;
			CpuTimePersent = cputime;
		}

        private void Set<T, TProperty>(ref T field, T newValue, Expression<Func<TProperty>> property)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) return;
            var memberExpression = property.Body as MemberExpression;
            field = newValue;
            OnPropertyChanged(memberExpression.Member.Name);
        }

        public static ProcessModel CompareChanger(ProcessModel dest, Process sourse, string persentload)
		{
			dest.Id	= sourse.Id;
            dest.Name = sourse.ProcessName;
		    dest.Memory =  Math.Round((sourse.WorkingSet64/1024f)/1024f,3);
            dest.Treads = sourse.Threads.Count;
            dest.CpuTime = Taskmsg.GetProcessTime(sourse.Id);
            dest.CpuTimePersent = persentload;
            return dest;
		}

		public event PropertyChangedEventHandler PropertyChanged;
        
		private void OnPropertyChanged(string propertyName = null)
		{
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

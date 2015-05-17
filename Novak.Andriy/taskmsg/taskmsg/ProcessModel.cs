using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using taskmsg.Annotations;

namespace taskmsg
{
	public class ProcessModel : INotifyPropertyChanged
	{
		public int		Id		{ private set; get; }
		public string	Name	{ private set; get; }
		public double	Memory  { private set; get; }
		public int		Treads  { private set; get; }
		public string CpuTime { private set; get; }

		public ProcessModel(int id, string name, double memory, int treads, TimeSpan ms)
		{
			Id = id;
			Name = name;
			Memory = Math.Round(memory,3);
			Treads = treads;
			CpuTime = ms.ToString(@"hh\:mm\:ss");
			
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

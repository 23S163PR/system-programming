using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace observable_collection
{
	public class Foo : INotifyPropertyChanged
	{
		private int _id;
		private string _name;

		public int Id
		{
			get { return _id; }
			set
			{
				if (_id == value) return;

				_id = value;
				InvokePropertyChanged();
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name == value) return;

				_name = value;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace observable_collection
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
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

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

		private string DebuggerDisplay
		{
			get { return string.Format("Id = {0}; Name = {1}", Id, Name); }
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

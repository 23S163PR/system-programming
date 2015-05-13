using System.Windows.Controls;

namespace taskmsg.views
{
	public partial class VTreeListView : UserControl
	{
		private readonly Taskmsg _tmsg;
		public VTreeListView()
		{
			InitializeComponent();
			_tmsg = new Taskmsg();
			//TreeList.Columns = _tmsg.Processes;
			//TreeList.DataContext = _tmsg.Processes;
		}
	}
}

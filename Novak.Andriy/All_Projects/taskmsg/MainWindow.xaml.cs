using System.Windows;
using taskmsg.views;

namespace taskmsg
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			TasksGrid.Children.Add(new DataGridControl());
		}
	}
}

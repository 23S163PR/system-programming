using System.Windows;
using ProcessManager.views;

namespace TaskManager
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

using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using taskmsg.views;

namespace taskmsg
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			//if (!load())
			//{
			//	Application.Current.Shutdown();
			//}
			
			InitializeComponent();

			TasksGrid.Children.Add(new DataGridControl());
		}

		private bool load()
		{
			var identity = WindowsIdentity.GetCurrent();
			if (identity != null)
			{
				var principal = new WindowsPrincipal(identity);
				if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					MessageBox.Show("Вы должны запустить программу под правами администратора. Программа будет закрыта");
					return false;
				}
			}
			return true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace synchronization_context
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep(2000);

				return "Have 20!";
			})
			.ContinueWith((task) =>
			{
				button1.Content = task.Result;
			}, TaskScheduler.FromCurrentSynchronizationContext());
					
		}
	}
}

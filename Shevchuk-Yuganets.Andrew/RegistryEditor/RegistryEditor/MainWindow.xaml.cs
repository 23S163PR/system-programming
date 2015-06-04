using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;

namespace RegistryEditor
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

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			// creating registry branches
			var classesRoot = new TreeViewItem
			{
				Tag = Registry.ClassesRoot,
				Header = Registry.ClassesRoot.Name,
				Items = { "*" }
			};

			var currentUser = new TreeViewItem
			{
				Tag = Registry.CurrentUser,
				Header = Registry.CurrentUser.Name,
				Items = { "*" }
			};

			var localMachine = new TreeViewItem
			{
				Tag = Registry.LocalMachine,
				Header = Registry.LocalMachine.Name,
				Items = { "*" }
			};

			var users = new TreeViewItem
			{
				Tag = Registry.Users,
				Header = Registry.Users.Name,
				Items = { "*" }
			};

			var currentConfig = new TreeViewItem
			{
				Tag = Registry.CurrentConfig,
				Header = Registry.CurrentConfig.Name,
				Items = { "*" }
			};

			var computer = new TreeViewItem
			{
				Tag = null,
				Header = "Computer",
				Items = { classesRoot, currentUser, localMachine, users, currentConfig } // add registry branches to default tree view item "computer"
			};
			RegistryTreeView.Items.Add(computer);
		}


		private void TreeViewItem_OnExpanded(object sender, RoutedEventArgs e)
		{
			var treeViewItem = e.OriginalSource as TreeViewItem;

			if (treeViewItem == null) return;
			if (treeViewItem.Tag == null) // check if expanded item not a "computer"
			{
				return;
			}
			else
			{
				// add new items from branch
			}
		}

		private void TreeViewItem_OnCollapsed(object sender, RoutedEventArgs e)
		{

		}
	}
}

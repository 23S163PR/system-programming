using System.Windows;

namespace RegEditor
{
	public partial class MainWindow : Window
	{
		private readonly RegistryEditor _registryEditor;
		public MainWindow()
		{
			InitializeComponent();
			
			_registryEditor = new RegistryEditor();
			foreach (var item in _registryEditor.Items)
			{
				RegistryKeys.Items.Add(item);
			}
		}

		private void RegistryKeys_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var item = RegistryKeys.SelectedItem as TreeItem;
			if (item == null) return;
			item.Items.Clear();
			_registryEditor.GetChildKeys(ref item);
		}

		private void RegistryKeys_OnExpanded(object sender, RoutedEventArgs e)
		{
			var item = sender as TreeItem;
			_registryEditor.GetChildKeys(ref item);
		}
	}
}

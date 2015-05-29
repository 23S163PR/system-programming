using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace RegEditor
{
	public partial class MainWindow : Window
	{
		private readonly RegistryEditor _registryEditor;
	    private RegistryValue? _registryValue;
		public MainWindow()
		{
			InitializeComponent();
		    _registryValue = null;
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
            item.ListItems.Clear();
            var res = RegistryEditor.GetChildKeys(item);
            if (res == null) return;
             item.ListItems.Clear();
            foreach (var bar in res)
		    {
                item.ListItems.Add(bar);
		    }
            DataGridInfo(item.Key);
		}

	    private void DataGridInfo(RegistryKey key)
	    {
            var res = key.GetValueNames().Select(p => new RegistryValue
	        {
	            Name = p//.Any() ? p : "default"
                ,Type = key.GetValueKind(p)
                ,Value=key.GetValue(p)
	        });
	        InfoDataGrid.DataContext = res;
        }
        #region Comands

	    private void CreateKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CreateKey_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeItem = RegistryKeys.SelectedItem as TreeItem;
            if (treeItem == null) return;
            var key = treeItem.Key;
           // _registryEditor.CreateKey(key);
        }

	    private void UpdateKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UpdateKey_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           
        }

        private void DeleteKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteKey_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeItem = RegistryKeys.SelectedItem as TreeItem;
            if (treeItem == null) return;
            if (_registryValue != null) _registryEditor.DeleteKey(treeItem, _registryValue.Value.Name);
        }

	    #endregion

	    private void CtxMenu_OnOpened(object sender, RoutedEventArgs e)
	    {
	        if (InfoDataGrid.SelectedItem is RegistryValue)
	        {
                _registryValue = (InfoDataGrid.SelectedItem as RegistryValue?);
	        }    
	    }
	}
}

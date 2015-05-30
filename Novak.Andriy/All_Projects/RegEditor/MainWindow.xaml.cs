using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RegEditor.Usercontol;

namespace RegEditor
{
	public partial class MainWindow : Window
	{
		private readonly RegistryEditor _registryEditor;

	    private RegistryValue? _registryValue;

	    private TreeItem _ctxSelectedItem;
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
            _ctxSelectedItem = RegistryKeys.SelectedItem as TreeItem;
            if (_ctxSelectedItem == null) return;
            _ctxSelectedItem.ListItems.Clear();
            var res = RegistryEditor.GetChildKeys(_ctxSelectedItem);
            if (res == null) return;
            
            foreach (var bar in res)
		    {
                _ctxSelectedItem.ListItems.Add(bar);
		    }
            DataGridInfo(_ctxSelectedItem.Key);
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
        #region Comands CanExecuted
        private void CreateKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UpdateKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteKeyValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UpdateKeyValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CreateKeyValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        
        private void CreateKeyValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_ctxSelectedItem == null) return;
            var control = new RegistryValuesControl();
            WindowOperator.Create_Window(control, "Create Key Value", true /*is Modal window*/);
            if (!control.DialogResult) return;
            _registryEditor.CreateKeyValue(_ctxSelectedItem, control.RegistryValue);
        }

        private void UpdateKeyValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeItem = RegistryKeys.SelectedItem as TreeItem;
            if (treeItem == null || _registryValue == null) return;
            var control = new RegistryValuesControl(_registryValue);
            WindowOperator.Create_Window(control, "Create Key Value", true /*is Modal window*/);
            if (!control.DialogResult) return;
            _registryEditor.UpdateKeyValue(treeItem, control.RegistryValue, _registryValue.Value.Name);
        }

        private void DeleteKeyValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var treeItem = RegistryKeys.SelectedItem as TreeItem;
            if (treeItem == null) return;
            try
            {
                if (_registryValue != null) _registryEditor.DeleteKeyValue(treeItem, _registryValue.Value.Name);
            }
            catch (ArgumentOutOfRangeException m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void CreateKey_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_ctxSelectedItem == null) return;
            var control = new RegistryKeyControl();
            WindowOperator.Create_Window(control, "Create Registry Key", true /*is Modal window*/);
            if (!control.DialogResult) return;
            _registryEditor.CreateKey(_ctxSelectedItem, control.KeyName);
        }
        
        private void UpdateKey_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_ctxSelectedItem == null) return;
            var control = new RegistryKeyControl(_ctxSelectedItem.Title);
            WindowOperator.Create_Window(control, "Rename Registry Key", true /*is Modal window*/);
            if (!control.DialogResult) return;
            _registryEditor.UpdateKey(_ctxSelectedItem, control.KeyName);  
        }

        private void DeleteKey_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (_ctxSelectedItem == null) return;
            _registryEditor.DeleteRegistryKey(_ctxSelectedItem);
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

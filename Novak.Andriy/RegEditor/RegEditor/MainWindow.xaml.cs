﻿using System.Linq;
using System.Windows;
using Microsoft.Win32;

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
            item.ListItems.Clear();
            var res = _registryEditor.GetChildKeys(item);
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
	        var res = key.GetValueNames().Select(p => new
	        {
	            Name = p//.Any() ? p : "default"
                ,Type = key.GetValueKind(p)
                ,Value=key.GetValue(p)
	        });
	        InfoDataGrid.DataContext = res;
            
	    }
	}
}

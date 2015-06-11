using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Win32;

namespace RegEditor
{
	class TreeItem : TreeViewItem
	{
		public string Title { get; set;}
	    public RegistryKey Key { get; private set; }

	    public ObservableCollection<TreeItem> ListItems { get; set; }

	    public TreeItem(RegistryKey key, string title)
	    {
	        Header = Title = title;
			Key = key;
            ListItems = new ObservableCollection<TreeItem>();
	        ItemsSource = ListItems;
	    }  
	}
}



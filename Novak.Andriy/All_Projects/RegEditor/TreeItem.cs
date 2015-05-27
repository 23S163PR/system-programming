using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace RegEditor
{
	class TreeItem 
	{
		public string Title { get; set;}
	    public RegistryKey Key { get; private set; }

	    public ObservableCollection<TreeItem> ListItems { get; set; }

	    public TreeItem(RegistryKey key, string title)
	    {
	        Title = title;
			Key = key;
            ListItems = new ObservableCollection<TreeItem>();
	    }

	    
	}
}

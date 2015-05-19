using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace RegEditor
{
	class TreeItem
	{
		public string Title { get; set;}
		public RegistryKey Key { get; set; }

		public ObservableCollection<TreeItem> Items { get; set; }

		public TreeItem(RegistryKey key, string title)
		{
			Key = key;
			Title = title;
			Items = new ObservableCollection<TreeItem>();
		}	
	}
}

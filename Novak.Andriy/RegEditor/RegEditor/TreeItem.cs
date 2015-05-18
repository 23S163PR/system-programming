using System.Collections.ObjectModel;

namespace RegEditor
{
	class TreeItem
	{
		public string Title { get; set; }

		public ObservableCollection<TreeItem> Items { get; set; }

		public TreeItem()
		{
			Items = new ObservableCollection<TreeItem>();
		}	
	}
}

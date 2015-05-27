using System.Windows;
using System.Windows.Controls;

namespace taskmsg
{
	public class TreeListView : TreeView
	{
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListViewItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListViewItem;
		}

		#region Public Properties

		public GridViewColumnCollection Columns
		{
			get { return _columns ?? (_columns = new GridViewColumnCollection() ); }
			set { _columns = value; }
		}

		private GridViewColumnCollection _columns;

		#endregion
	}

	public class TreeListViewItem : TreeViewItem
	{
		public int Level
		{
			get
			{
				if (_level != -1) return _level;
				var parent = ItemsControlFromItemContainer(this) as TreeListViewItem;
				_level = (parent != null) ? parent.Level + 1 : 0;
				return _level;
			}
		}


		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListViewItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListViewItem;
		}

		private int _level = -1;
	}
}

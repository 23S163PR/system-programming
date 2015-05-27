using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using Microsoft.Win32;

namespace RegEditor
{
	class RegistryEditor
	{
		private ObservableCollection<TreeItem> _items;
        public IEnumerable<TreeItem> Items { get { return _items; } }


		public RegistryEditor()
		{
		  _items = new ObservableCollection<TreeItem>();
			ReadRegistry();
		}

		private void ReadRegistry()
		{
			RegistryKeys(Registry.ClassesRoot);
			RegistryKeys(Registry.CurrentUser);
			RegistryKeys(Registry.LocalMachine);
			RegistryKeys(Registry.Users);
			RegistryKeys(Registry.CurrentConfig);
		}
        
		private void RegistryKeys(RegistryKey registryKey)
		{
			var root = new TreeItem(registryKey, registryKey.Name);
            foreach (var keys in GetChildKeys(root))
		    {
                root.ListItems.Add(keys);
		    }
		    _items.Add(root);
		}

        public IEnumerable<TreeItem> GetChildKeys(TreeItem key)
        {
            var items = new ObservableCollection<TreeItem>();
            foreach (var name in key.Key.GetSubKeyNames())
            {
                try
                {
                    var childKey = key.Key.OpenSubKey(name);//open child keys
                    var item = new TreeItem(childKey, name);
                    if (childKey != null)
                    {
                        foreach (var citem in childKey.GetSubKeyNames())
                        {
                            var subchild = childKey.OpenSubKey(citem);
                            var sybitem = new TreeItem(subchild, citem);
                            item.ListItems.Add(sybitem);
                            item.ListItems.Add(sybitem);
                        }
                    }
                    items.Add(item);
                }
                catch (Exception)
                {
                    //this exception for system keys
                }
            }
            return items;
        }

        //public void CreateKey()
	}
}

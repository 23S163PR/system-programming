using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;

namespace RegEditor
{
    
    struct RegistryValue
    {
        public RegistryKey Key { get; set; }
        public string Name { get; set; }
        public RegistryValueKind Type { get; set; }
        public object Value { get; set; }
    }

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
                {//todo Acces control #p1 some keys dont have access control 
                    var childKey = key.Key.OpenSubKey(name,true /*open key with read & write acces*/);
                    var item = new TreeItem(childKey, name);
                    if (childKey != null)
                    {
                        foreach (var citem in childKey.GetSubKeyNames())
                        {
                            //var tt=childKey.OpenSubKey(citem).GetAccessControl();
                            //todo Acces control #p1
                            var subchild = childKey.OpenSubKey(citem, true /*open key with read & write acces*/);
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

        public static void CreateKey(RegistryKey key)
        {
            try
            {
                key.CreateSubKey("WWW123");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
	    public static void DeleteKey(RegistryKey key, string keyName)
	    { 
	        try
	        {
	            key.DeleteSubKey(keyName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
	    }
	}
}

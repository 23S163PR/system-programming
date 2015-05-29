using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace RegEditor
{
    
    struct RegistryValue
    {
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

        public static IEnumerable<TreeItem> GetChildKeys(TreeItem key)
        {
            var items = new ObservableCollection<TreeItem>();
            foreach (var name in key.Key.GetSubKeyNames())
            {
                try
                {
                    var childKey = key.Key.OpenSubKey(name);
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
                    //this exception may be if dont access to system registry keys 
                }
            }
            return items;
        }

        private RegistryKey OpenCurentKeyWithWriteAccess(TreeItem key)
        {
            var regexpRoot = new Regex(@"(^[\w]*)");
            var regexpParentKey = new Regex(@"([\\][\w]*)*([\\])");
            var root = regexpRoot.Match(key.Key.Name);
            RegistryKey rooKey = null;
            switch (root.Value)
            {
                case "HKEY_CLASSES_ROOT":
                    rooKey = Registry.ClassesRoot;
                    break;
                case "HKEY_CURRENT_USER":
                    rooKey = Registry.CurrentUser;
                    break;
                case "HKEY_LOCAL_MACHINE":
                    rooKey = Registry.LocalMachine;
                    break;
                case "HKEY_USERS":
                    rooKey = Registry.Users;
                    break;
                case "HKEY_CURRENT_CONFIG":
                    rooKey = Registry.CurrentConfig;
                    break;  
            }
            var parent = regexpParentKey.Match(key.Key.Name);
            return rooKey.OpenSubKey(parent.Groups[1].Value.Remove(0, 1), true/*open with writable*/);
        }

	    public void CreateKey(TreeItem key)
        {
            try
            {
              //  key.CreateSubKey("WWW123");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
	    public void DeleteKey(TreeItem key, string keyValue = null)
	    {
	        try
	        {
                var curent = OpenCurentKeyWithWriteAccess(key).OpenSubKey(key.Title, true/*open writable*/);
	            if (curent == null) return;
	            if (keyValue != null)
	            {
                    curent.DeleteValue(keyValue);
	                return;
	            }
	            if (!curent.GetSubKeyNames().Contains(key.Title)) return;
	            var curentChild = curent.OpenSubKey(key.Title, true);
	            if (curentChild != null && curentChild.SubKeyCount > 0)
	            {
	                curent.DeleteSubKeyTree(key.Title);
	            }
	            else
	            {
	                curent.DeleteSubKey(key.Title);
	            }
	        }
	        catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
	    }
	}
}

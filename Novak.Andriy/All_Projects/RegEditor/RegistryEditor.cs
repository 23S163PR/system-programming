using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace RegEditor
{
    public struct RegistryValue
    {
        public string Name { get; set; }
        public RegistryValueKind Type { get; set; }
        public object Value { get; set; }
    }

	class RegistryEditor
	{
		private readonly ObservableCollection<TreeItem> _items;
        public IEnumerable<TreeItem> Items { get { return _items; } }

        public RegistryEditor()
        {
            _items = new ObservableCollection<TreeItem>();

            //read registry keys
           // RegistryKeys(Registry.ClassesRoot);
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
                catch (IOException)
                {
                    //this exception may be if dont access to system registry keys 
                    continue;
                }
                catch (SecurityException)
                {
                    continue;
                }
            }
            return items;
        }

        private static RegistryKey OpenKeyWithWriteAccess(TreeItem key, bool isParentKey = false)
        {
            var regexpRoot = new Regex(@"(^[\w]*)");
            var regexpParentKey = new Regex(@"([\\][\w]*)*([\\])");
            var root = regexpRoot.Match(key.Key.Name);
            RegistryKey rootKey = null;
            switch (root.Value)
            {
                case "HKEY_CLASSES_ROOT":
                    rootKey = Registry.ClassesRoot;
                    break;
                case "HKEY_CURRENT_USER":
                    rootKey = Registry.CurrentUser;
                    break;
                case "HKEY_LOCAL_MACHINE":
                    rootKey = Registry.LocalMachine;
                    break;
                case "HKEY_USERS":
                    rootKey = Registry.Users;
                    break;
                case "HKEY_CURRENT_CONFIG":
                    rootKey = Registry.CurrentConfig;
                    break;  
            }
            if (key.Title.Equals(root.Value)) return rootKey;

            var parent = regexpParentKey.Match(key.Key.Name);
            var corectParent = TrimFirstBackSlash(parent.Value);

            if (!isParentKey)
            {
                 return !parent.Groups[1].Value.Any() 
                     ? rootKey.OpenSubKey(key.Title, true /*open writable*/)
                : rootKey.OpenSubKey(corectParent)
                        .OpenSubKey(key.Title, true /*open writable*/);
            }

            return !parent.Groups[1].Value.Any()
                ? rootKey
                : rootKey.OpenSubKey(corectParent, true /*open writable*/);         
        }

	    private static string TrimFirstBackSlash(string text)
	    {
	       return text.Remove(0, 1);
	    }

	    public void CreateKey(TreeItem key, string keyName)
	    {
	        var curent = OpenKeyWithWriteAccess(key);
	        curent.CreateSubKey(keyName);
	        if (!_items.Contains(key)) return;
	        key.ListItems.Clear();
	        foreach (var item in GetChildKeys(key))
	        {
	            key.ListItems.Add(item);
	        }
	    }

	    public void CreateKeyValue(TreeItem key, RegistryValue value)
	    {
	        var curent = OpenKeyWithWriteAccess(key);
            curent.SetValue(value.Name, value.Value, value.Type);
	    }

	    public void UpdateKeyValue(TreeItem key, RegistryValue newValue, string oldValueName)
	    {
	        var curent = OpenKeyWithWriteAccess(key);
            if (curent == null) return;
            curent.DeleteValue(oldValueName);
            curent.SetValue(newValue.Name, newValue.Value, newValue.Type);
	    }

        public void UpdateKey(TreeItem key, string newKeyName)
        {
            var parent = OpenKeyWithWriteAccess(key,true);
            CopyKey(parent, key.Title, newKeyName);
            parent.DeleteSubKeyTree(key.Title);
        }

        /// <summary>
        /// Copy a registry key.  The parentKey must be writeable.
        /// </summary>
        private void CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            //Create new key
            var destinationKey = parentKey.CreateSubKey(newKeyName);

            //Open the sourceKey we are copying from
            var sourceKey = parentKey.OpenSubKey(keyNameToCopy);

            RecurseCopyKey(sourceKey, destinationKey);
        }

        private void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            //copy all the values
            foreach (var valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            //For Each subKey 
            //Create a new subKey in destinationKey 
            //Call myself 
            foreach (var sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                var sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                var destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }

	    public void DeleteKeyValue(TreeItem key, string keyValue)
	    {
            var curent = OpenKeyWithWriteAccess(key);
            if (curent == null || keyValue == null) return;
	        curent.DeleteValue(keyValue);
	    }

	    public void DeleteRegistryKey(TreeItem key)
	    {
            if (_items.Contains(key)) return;
            var parent = OpenKeyWithWriteAccess(key, true /*Get parent key in curent key*/);
            var curentChild = parent.OpenSubKey(key.Title);
            if (curentChild != null && curentChild.SubKeyCount > 0)
            {
                parent.DeleteSubKeyTree(key.Title);
            }
            else
            {
                parent.DeleteSubKey(key.Title);
            }

	    }
	}
}

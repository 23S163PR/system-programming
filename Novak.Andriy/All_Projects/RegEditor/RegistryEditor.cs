using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var task = new Task<ObservableCollection<TreeItem>>(() =>
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
            });
            task.Start(TaskScheduler.Current);
            return task.Result;
        }

        private static RegistryKey OpenCurentKeyWithWriteAccess(TreeItem key, bool isParentKey = false)
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
            if (!isParentKey)
            {
                 return !parent.Groups[1].Value.Any() 
                     ? rootKey.OpenSubKey(key.Title, true /*open writable*/)
                : rootKey.OpenSubKey(parent.Groups[1].Value.Remove(0, 1))
                        .OpenSubKey(key.Title, true /*open writable*/);
            }

            return !parent.Groups[1].Value.Any()
                ? rootKey
                : rootKey.OpenSubKey(parent.Groups[1].Value.Remove(0, 1), true /*open writable*/);         
        }

	    public void CreateKey(TreeItem key, string keyName)
	    {
	        var curent = OpenCurentKeyWithWriteAccess(key);
	        curent.CreateSubKey(keyName);
	    }

	    public void CreateKeyValue(TreeItem key, RegistryValue value)
	    {
	        var curent = OpenCurentKeyWithWriteAccess(key);
            curent.SetValue(value.Name, value.Value, value.Type);
	    }

	    public void UpdateKeyValue(TreeItem key, RegistryValue newValue, string oldValueName)
	    {
	        var curent = OpenCurentKeyWithWriteAccess(key);
            if (curent == null) return;
            curent.DeleteValue(oldValueName);
            curent.SetValue(newValue.Name, newValue.Value, newValue.Type);
	    }

        public void UpdateKey(TreeItem key, string newKeyName)
        {
            var parent = OpenCurentKeyWithWriteAccess(key,true);
            CopyKey(parent, key.Title, newKeyName);
            parent.DeleteSubKeyTree(key.Title);
        }

	    private void RenameSubKey(RegistryKey parentKey,
          string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
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
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                var sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                var destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }

	    public void DeleteKeyValue(TreeItem key, string keyValue)
	    {
            var curent = OpenCurentKeyWithWriteAccess(key);
            if (curent == null || keyValue == null) return;
	        curent.DeleteValue(keyValue);
	    }

	    public void DeleteRegistryKey(TreeItem key)
	    {
            var parent = OpenCurentKeyWithWriteAccess(key, true /*Get parent key in curent key*/);
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

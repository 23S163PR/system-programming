using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;

namespace RegEditor
{
	class RegistryEditor
	{
		private ObservableCollection<TreeItem> _items;
		public ObservableCollection<TreeItem> Items { get { return _items; }  }


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

		

		//#region Get Keys
		private void RegistryKeys(RegistryKey registryKey)
		{
			var root = new TreeItem(registryKey, registryKey.Name);
			GetChildKeys(ref root);
			_items.Add(root);
		}

		public void GetChildKeys(ref TreeItem key)
		{
			foreach (var name in key.Key.GetSubKeyNames())
			{
				try
				{
					var childKey = key.Key.OpenSubKey(name);//open child keys
					//if (key.Items.FirstOrDefault(p => p.Title.Equals(name)) == null) // check if child list conteins new element
					//{
					var item = new TreeItem(childKey, name);
					//}
					if (childKey != null)
					{
						foreach (var citem in childKey.GetSubKeyNames())
						{
							var subchild = childKey.OpenSubKey(citem);
							//if (item.Items.FirstOrDefault(p => p.Title.Equals(citem)) != null) continue;
							var sybitem = new TreeItem(subchild, citem);
							item.Items.Add(sybitem);
						}
					}
					key.Items.Add(item);
				}
				catch (Exception)
				{
					//this exception for system keys
				}
			}
		}
	}
}

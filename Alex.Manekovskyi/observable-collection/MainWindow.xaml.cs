using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace observable_collection
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Random _random;
		private ObservableCollection<Foo> _foos;
		public ObservableCollection<Foo> Foos { get { return _foos;  } }

		public MainWindow()
		{
			InitializeComponent();

			_random = new Random();
			_foos = new ObservableCollection<Foo>();

			this.DataContext = this;
		}

		private void AddRandomItem(object sender, RoutedEventArgs e)
		{
			var randomIndex = _random.Next(0, Foos.Count);
			Foos.Insert(randomIndex, new Foo
			{
				Id = _random.Next(int.MaxValue),
				Name = Guid.NewGuid().ToString()
			});
		}

		private void RemoveRandomItem(object sender, RoutedEventArgs e)
		{
			var randomIndex = _random.Next(0, Foos.Count);
			Foos.RemoveAt(randomIndex);
		}

		private void UpdateRandomItem(object sender, RoutedEventArgs e)
		{
			var randomIndex = _random.Next(0, Foos.Count);
			var item = Foos[randomIndex];

			item.Name = Guid.NewGuid().ToString();
			item.Id = _random.Next(int.MaxValue);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Groups.Virtualizing;

namespace Groups
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Employee> _listAllEmployees = new List<Employee>();
        private List<string> _nameGrops = new List<string>();
        private Dictionary<string, List<Employee>> _groups = new Dictionary<string, List<Employee>>();
        private Dictionary<string, Func<Employee, bool>> _predicateGroups;
        private int _pageSizeForVirtualizing;


        public MainWindow()
        {
            InitVariable();
            InitializeComponent();
            ModalLoader.HiddenLoader();
        }

        public void InitVariable()
        {
            _nameGrops = new List<string> { "Group_1", "Group_2", "Group_3", "Group_4" };
            _predicateGroups = new Dictionary<string, Func<Employee, bool>>
            {
                {_nameGrops[0], e => e.AgeInYears >= 21 && e.Salary > 15000},
                {_nameGrops[1], e => e.Gender == Gender.Woman && (e.Name.Split(' ')[1].StartsWith("A") || e.Name.Split(' ')[1].StartsWith("А"))},
                {_nameGrops[2], e => e.Gender == Gender.Men && e.Salary > 20000},
                {_nameGrops[3], e => e.Gender == Gender.Transgender && e.AgeInYears >= 50}
            }; 
            _pageSizeForVirtualizing = 100;
            foreach (var name in _nameGrops)
            {
                 _groups.Add(name,new List<Employee>());
            }
        }
        #region Event

        private void LoadXml_OnClick(object sender, RoutedEventArgs e)
        {
            ModalLoader.ShowLoader();
            var initial = new Task(LoadFromXml);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.Invoke(DataGrigVirtualizing));
            initial.ContinueWith((t) => Dispatcher.Invoke(ModalLoader.HiddenLoader));
        }

        private void SaveXml_OnClick(object sender, RoutedEventArgs e)
        {
            ModalLoader.ShowLoader();
            var initial = new Task(SaveToXml);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.Invoke(ModalLoader.HiddenLoader));
        }
       
        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            ModalLoader.ShowLoader();
            var initial = new Task(GenerateEmployyes);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.Invoke(InitGroups));
            initial.ContinueWith((t) => Dispatcher.Invoke(DataGrigVirtualizing));
            initial.ContinueWith((t) => Dispatcher.Invoke(ModalLoader.HiddenLoader));
        }

        private void LoadNamesFromSite_OnClick(object sender, RoutedEventArgs e)
        {
            ModalLoader.ShowLoader();
            var initial = new Task(LoadNamesFromSite);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.Invoke(ModalLoader.HiddenLoader));
        }

        #endregion



        #region Function 
        private void SaveToXml()
        {
            Parallel.ForEach(_nameGrops, name =>
            {
                ManageEmployees.SavetoXml(name.GetNameXml(), _groups[name]);
            });
        }

        private void LoadFromXml()
        {
            Parallel.ForEach(_nameGrops, name =>
            {
                _groups[name] = ManageEmployees.DownloadFromXml(name.GetNameXml());
            });
        }
        
        private void GenerateEmployyes()
        {
            _listAllEmployees.Clear();
            var names = ManageNames.DeserializingNamesFromXml();
            _listAllEmployees.AddRange(ManageEmployees.ParseOfRealData(names));
            int countTransgender;
            if(int.TryParse(Dispatcher.Invoke(() => ValueCountPage.Text),out countTransgender))
                _listAllEmployees.AddRange(ManageEmployees.CreateMythicalData(countTransgender*5));
        }

        private void InitGroups()
        {
            Parallel.ForEach(_nameGrops, name =>
            {
                _groups[name] = _listAllEmployees.AsParallel().Where(_predicateGroups[name]).SortEmployees(FieldEmployees.AgeInYears, true);
            });
        }

        private void DataGrigVirtualizing()
        {
            var providers = _nameGrops.ToDictionary(name => name, name => new EmployeeProvider(_groups[name].Count));
            
            ListViewGroup1.DataContext = new VirtualizingCollection<Employee>(_groups["Group_1"], providers["Group_1"], _pageSizeForVirtualizing);
            ListViewGroup2.DataContext = new VirtualizingCollection<Employee>(_groups["Group_2"], providers["Group_2"], _pageSizeForVirtualizing);
            ListViewGroup3.DataContext = new VirtualizingCollection<Employee>(_groups["Group_3"], providers["Group_3"], _pageSizeForVirtualizing);
            ListViewGroup4.DataContext = new VirtualizingCollection<Employee>(_groups["Group_4"], providers["Group_4"], _pageSizeForVirtualizing);
        }

        private void LoadNamesFromSite()
        {
            int count;
            if (int.TryParse(Dispatcher.Invoke(() => ValueCountPage.Text), out count))
            {
                var names = ManageNames.ParseSiteForNamesList(count);
                ManageNames.SeserializingNamesToXml(names);
            }
        }

        #endregion

    }
}

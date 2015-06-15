using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Faker;
using HtmlAgilityPack;
using System.Windows.Threading;
namespace Groups
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Employee> _listAllEmployees;
        private Dictionary<string, List<Employee>> _groups = new Dictionary<string, List<Employee>>();
        private readonly List<string> _nameGrops = new List<string>() {"Group_1", "Group_2", "Group_3", "Group_4"};
        private Dictionary<string, Func<Employee, bool>> _predicateGroups;

        public MainWindow()
        {
            InitializeComponent();
            _listAllEmployees = new List<Employee>();
            _predicateGroups = new Dictionary<string, Func<Employee, bool>>
            {
                {_nameGrops[0], e => e.AgeInYears >= 21 && e.Salary > 15000},
                {_nameGrops[1], e => e.Gender == Gender.Woman && (e.Name.StartsWith("A") || e.Name.StartsWith("А"))},
                {_nameGrops[2], e => e.Gender == Gender.Men && e.Salary > 20000},
                {_nameGrops[3], e => e.Gender == Gender.Transgender && e.AgeInYears >= 50}
            };
            ModalLoader.HiddenLoader();
        }


        private void SaveXml_OnClick(object sender, RoutedEventArgs e)
        {
            //SortGroups();
            foreach (var name in _nameGrops)
            {
                ManageEmployees.SavetoXml(String.Format(name, ".xml"), _groups[name]);                
            }
        }
       

        private void LoadXml_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var name in _nameGrops)
            {
                _groups[name] = ManageEmployees.DownloadFromXml(String.Format(name, ".xml"));
            }
            DataGrigInit();
            SortGroups();
        }

        private void SortGroups()
        {
            foreach (var name in _nameGrops)
            {
                _groups[name] = _groups[name].SortEmployees(FieldEmployees.AgeInYears, true);
            }
        }

        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO Progress Bar
            var initial = new Task(GenerateEmployyes);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.BeginInvoke(new ThreadStart(DataGrigInit)));
        }

        private void GenerateEmployyes()
        {
             var names = ManageNames.DeserializingNamesFromXml();
             _listAllEmployees.AddRange(ManageEmployees.ParseOfRealData(names));
             _listAllEmployees.AddRange(ManageEmployees.CreateMythicalData());
            InitGroups();
        }

        private void InitGroups()
        {
            //_group1.Clear();
            //_group2.Clear();
            //_group3.Clear();
            //_group4.Clear();
            //var time = new Stopwatch();
            //time.Start();
            //_group1.AddRange(_listAllEmployees.AsParallel().Where(_predicateGroup1));
            //_group2.AddRange(_listAllEmployees.AsParallel().Where(_predicateGroup2));
            //_group3.AddRange(_listAllEmployees.AsParallel().Where(_predicateGroup3));
            //_group4.AddRange(_listAllEmployees.AsParallel().Where(_predicateGroup4));
            //time.Stop();
            //_group1.TrimExcess();
            //_group2.TrimExcess();
            //_group3.TrimExcess();
            //_group4.TrimExcess();
            //MessageBox.Show(String.Format(
            //    "Сортировка {0} работников по групам зайняло {1} милесекунд",
            //    _listAllEmployees.Count,
                //time.ElapsedMilliseconds));
        }

        private void DataGrigInit()
        {
            SortGroups();
            //DataGridGroup1.ItemsSource = _group1;
            //DataGridGroup2.ItemsSource = _group2;
            //DataGridGroup3.ItemsSource = _group3;
            //DataGridGroup4.ItemsSource = _group4;
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            ModalLoader.ShowLoader();
            var initial = new Task(ManageNames.SeserializingNamesToXml);
            initial.Start();
            initial.ContinueWith((t) => Dispatcher.BeginInvoke(new ThreadStart(ModalLoader.HiddenLoader)));
            
        }
    }
}

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
using m = WpfApp3.models;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Group.xaml
    /// </summary>
    public partial class Group : Page
    {
        FieldChecker FieldChecker;
        ApplicationContext db;
        ObservableCollection<m.GroupEl> memberData;
        List<m.Subdivision> subList;

        public Group()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<m.GroupEl>();
            subList = new List<m.Subdivision>();
            FieldChecker = new FieldChecker();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();

            var groups = from gr in db.Groups
                         join podr in db.Subdivisions on gr.SubdivisionId equals podr.Id
                         select new
                         {
                             Id = gr.Id,
                             Name = gr.Name,
                             SubdivisionId = gr.SubdivisionId,
                             SubNumber = podr.Number
                         };

            foreach (var item in groups)
                memberData.Add(new m.GroupEl { Id = item.Id, Name = item.Name, SubdivisionId = item.SubdivisionId, SubNumber = item.SubNumber });

            Grid.DataContext = memberData;


            subList = db.Subdivisions.ToList();

            subdvCb.DisplayMemberPath = "Number";
            subdvCb.SelectedValuePath = "Id";
            subdvCb.ItemsSource = subList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Teacher());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Audit());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Podr());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Disciplinesp());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Table());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            m.GroupEl items = Grid.SelectedItem as m.GroupEl;

            m.Group cur = db.Groups.FirstOrDefault(c => c.Id == items.Id);

            db.Groups.Remove(cur);
            db.SaveChanges();

            LoadData();

            if (subdvCb.SelectedIndex > -1)
                subdvCb.SelectedIndex = -1;
            if (groupTb.Text != "")
                groupTb.Text = "";
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            if (groupTb.Text != "" && subdvCb.SelectedIndex > -1)
            {
                if (FieldChecker.DeleteSpaces(groupTb.Text))
                {
                    var uniqField = db.Groups.FirstOrDefault(p => p.Name == groupTb.Text);

                    if (uniqField != null)
                    {
                        MessageBox.Show("Такая запись уже существует");
                        return;
                    }

                    m.GroupEl gr = Grid.SelectedItem as m.GroupEl;
                    m.Subdivision subdv = subdvCb.SelectedItem as m.Subdivision;

                    m.Subdivision curSbdv = db.Subdivisions.FirstOrDefault(c => c.Id == subdv.Id);
                    m.Group curGr = db.Groups.FirstOrDefault(c => c.Id == gr.Id);

                    curGr.Name = groupTb.Text;
                    curGr.Subdivision = curSbdv;

                    db.Groups.Update(curGr);
                    db.SaveChanges();

                    LoadData();

                    if (groupTb.BorderBrush == Brushes.Red)
                        groupTb.BorderBrush = Brushes.LightGray;
                    if (subdvCb.BorderBrush == Brushes.Red)
                        subdvCb.BorderBrush = Brushes.LightGray;

                    groupTb.Text = "";
                    subdvCb.SelectedIndex = -1;
                }
                else
                    MessageBox.Show("В названии группы должны отсутствовать пробелы");
            }
            else
            {
                if (groupTb.Text == "")
                    groupTb.BorderBrush = Brushes.Red;
                if (subdvCb.SelectedIndex > -1)
                    subdvCb.BorderBrush = Brushes.Red;
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (groupTb.Text != "" && subdvCb.SelectedIndex > -1)
            {
                if (FieldChecker.DeleteSpaces(groupTb.Text))
                {
                    m.Subdivision selectedSub = subdvCb.SelectedItem as m.Subdivision;
                    var curSub = db.Subdivisions.FirstOrDefault(p => p.Id == selectedSub.Id);

                    var uniqField = db.Groups.FirstOrDefault(p => p.Name == groupTb.Text);

                    if (uniqField != null)
                    {
                        MessageBox.Show("Такая запись уже существует");
                        return;
                    }

                    m.Group subdivisin = new m.Group { Name = groupTb.Text, Subdivision = curSub };
                    db.Groups.Add(subdivisin);
                    db.SaveChanges();

                    LoadData();

                    if (groupTb.BorderBrush == Brushes.Red)
                        groupTb.BorderBrush = Brushes.LightGray;
                    if (subdvCb.BorderBrush == Brushes.Red)
                        subdvCb.BorderBrush = Brushes.LightGray;

                    subdvCb.SelectedIndex = -1;
                    groupTb.Text = "";
                }
                else
                    MessageBox.Show("В названии группы должны отсутствовать пробелы");
            }
            else
            {
                if (groupTb.Text == "")
                    groupTb.BorderBrush = Brushes.Red;
                if (subdvCb.SelectedIndex < 0)
                    subdvCb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            m.GroupEl item = Grid.SelectedItem as m.GroupEl;

            if (item != null)
            {
                groupTb.Text = item.Name;
                subdvCb.SelectedValue = item.SubdivisionId;
            }
            else
                return;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

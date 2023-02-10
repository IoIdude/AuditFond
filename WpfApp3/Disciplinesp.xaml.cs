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
using WpfApp3.models;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Disciplinesp.xaml
    /// </summary>
    public partial class Disciplinesp : Page
    {
        ApplicationContext db;
        ObservableCollection<Discipline> memberData;
        FieldChecker FieldChecker;

        public Disciplinesp()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<Discipline>();
            FieldChecker = new FieldChecker();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();

            var subdv = from podr in db.Disciplines
                        select new
                        {
                            Id = podr.Id,
                            Name = podr.Name
                        };

            foreach (var item in subdv)
                memberData.Add(new Discipline { Id = item.Id, Name = item.Name });

            Grid.DataContext = memberData;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            if (disTb.Text != "")
                disTb.Text = "";

            Discipline items = Grid.SelectedItem as Discipline;

            Discipline cur = db.Disciplines.FirstOrDefault(c => c.Id == items.Id);

            db.Disciplines.Remove(cur);
            db.SaveChanges();

            LoadData();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem == null)
            {
                MessageBox.Show("Выберите элемент из списка для удаления");
                return;
            }

            if (disTb.Text != "")
            {
                var uniqField = db.Disciplines.FirstOrDefault(p => p.Name == disTb.Text);

                if (uniqField != null)
                {
                    MessageBox.Show("Такая дисциплина уже существует");
                    return;
                }

                Discipline item = Grid.SelectedItem as Discipline;

                Discipline cur = db.Disciplines.FirstOrDefault(c => c.Id == item.Id);
                cur.Name = disTb.Text;

                db.Disciplines.Update(cur);
                db.SaveChanges();

                LoadData();

                if (disTb.BorderBrush == Brushes.Red)
                    disTb.BorderBrush = Brushes.LightGray;

                disTb.Text = "";
            }
            else
            {
                disTb.BorderBrush = Brushes.Red;
            }
        }

        private void CraeteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (disTb.Text != "")
            {
                var uniqField = db.Groups.FirstOrDefault(p => p.Name == disTb.Text);

                if (uniqField != null)
                {
                    MessageBox.Show("Такая дисциплина уже существует");
                    return;
                }

                Discipline discipline = new Discipline { Name = disTb.Text };
                db.Disciplines.Add(discipline);
                db.SaveChanges();

                LoadData();

                if (disTb.BorderBrush == Brushes.Red)
                    disTb.BorderBrush = Brushes.LightGray;

                disTb.Text = "";
            }
            else
            {
                disTb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Discipline item = Grid.SelectedItem as Discipline;

            if (item != null)
            {
                disTb.Text = item.Name;
            }
            else
                return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Teacher());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Audit());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Table());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Group());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Podr());
        } 

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

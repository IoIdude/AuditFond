using Google.Protobuf.WellKnownTypes;
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
    /// Логика взаимодействия для Podr.xaml
    /// </summary>
    public partial class Podr : Page
    {
        ApplicationContext db;
        ObservableCollection<Subdivision> memberData;
        FieldChecker FieldChecker;

        public Podr()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<Subdivision>();
            FieldChecker = new FieldChecker();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();

            var subdv = from podr in db.Subdivisions
                        select new
                        {
                            Id = podr.Id,
                            Number = podr.Number
                        };

            foreach (var item in subdv)
                memberData.Add(new Subdivision { Id = item.Id, Number = item.Number });

            Grid.DataContext = memberData;
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
            NavigationService.Navigate(new Group());
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

            if (tb.Text != "")
                tb.Text = "";

            Subdivision items = Grid.SelectedItem as Subdivision;

            Subdivision cur = db.Subdivisions.FirstOrDefault(c => c.Id == items.Id);

            db.Subdivisions.Remove(cur);
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

            if (tb.Text != "")
            {
                string number = FieldChecker.CheckPodrMask(tb.Text);
                var uniqField = db.Subdivisions.FirstOrDefault(p => p.Number == number);

                if (uniqField != null)
                {
                    MessageBox.Show("Такая запись уже существует");
                    return;
                }

                Subdivision item = Grid.SelectedItem as Subdivision;

                Subdivision cur = db.Subdivisions.FirstOrDefault(c => c.Id == item.Id);
                cur.Number = number;

                db.Subdivisions.Update(cur);
                db.SaveChanges();

                LoadData();

                if (tb.BorderBrush == Brushes.Red)
                    tb.BorderBrush = Brushes.LightGray;

                tb.Text = "";
            }
            else
            {
                tb.BorderBrush = Brushes.Red;
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text != "")
            {
                if (FieldChecker.CheckPodrCountOfNumbers(tb.Text))
                {
                    string number = FieldChecker.CheckPodrMask(tb.Text);

                    var uniqField = db.Groups.FirstOrDefault(p => p.Name == number);

                    if (uniqField != null)
                    {
                        MessageBox.Show("Такая запись уже существует");
                        return;
                    }

                    Subdivision subdivisin = new Subdivision { Number = number };
                    db.Subdivisions.Add(subdivisin);
                    db.SaveChanges();

                    LoadData();

                    if (tb.BorderBrush == Brushes.Red)
                        tb.BorderBrush = Brushes.LightGray;

                    tb.Text = "";
                }
                else
                    MessageBox.Show("В поле 'Подразделение' количество цифр должно быть равно нулю\n Пример: 090207");
            }
            else
            {
                tb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Subdivision item = Grid.SelectedItem as Subdivision;

            if (item != null)
            {
                string num = item.Number.Remove(2, 1);
                num = num.Remove(4, 1);
                tb.Text = num;
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

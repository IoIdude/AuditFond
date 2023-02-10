using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// Логика взаимодействия для Teacher.xaml
    /// </summary>
    public partial class Teacher : Page
    {
        ApplicationContext db;
        ObservableCollection<m.Teacher> memberData;
        FieldChecker FieldChecker;

        public Teacher()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<m.Teacher>();
            FieldChecker = new FieldChecker();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();

            var teacher = from teach in db.Teachers
                          select new
                          {
                              Id = teach.Id,
                              Surname = teach.Surname,
                              Name = teach.Name,
                              LastName = teach.LastName,
                              Login = teach.Login,
                              Password = teach.Password
                          };

            foreach (var item in teacher)
            {
                if (item.Name != "Admin")
                {
                    memberData.Add(new m.Teacher { Id = item.Id, Surname = item.Surname, Name = item.Name, LastName = item.LastName, Login = item.Login, Password = item.Password });
                }
            }

            Grid.DataContext = memberData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Audit());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Group());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Disciplinesp());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Podr());
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

            if (lastNameTb.Text != "")
                lastNameTb.Text = "";
            if (nameTb.Text != "")
                nameTb.Text = "";
            if (surnameTb.Text != "")
                surnameTb.Text = "";
            if (loginTb.Text != "")
                loginTb.Text = "";
            if (passwordTb.Text != "")
                passwordTb.Text = "";

            m.Teacher items = Grid.SelectedItem as m.Teacher;

            m.Teacher cur = db.Teachers.FirstOrDefault(c => c.Id == items.Id);

            db.Teachers.Remove(cur);
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

            if (nameTb.Text != "" && surnameTb.Text != "" && lastNameTb.Text != "")
            {
                if (FieldChecker.CheckTeacher(surnameTb.Text) && FieldChecker.CheckTeacher(nameTb.Text) && FieldChecker.CheckTeacher(lastNameTb.Text))
                {
                    if (FieldChecker.DeleteSpaces(surnameTb.Text) && FieldChecker.DeleteSpaces(lastNameTb.Text) && FieldChecker.DeleteSpaces(nameTb.Text))
                    {
                        m.Teacher items = Grid.SelectedItem as m.Teacher;

                        m.Teacher cur = db.Teachers.FirstOrDefault(c => c.Id == items.Id);
                        cur.Name = nameTb.Text;
                        cur.Surname = surnameTb.Text;
                        cur.LastName = lastNameTb.Text;

                        if (loginTb.Text != "" && passwordTb.Text != "")
                        {
                            cur.Password = passwordTb.Text;
                            cur.Login = loginTb.Text;
                        }

                        db.Teachers.Update(cur);
                        db.SaveChanges();

                        LoadData();

                        if (nameTb.BorderBrush == Brushes.Red)
                            nameTb.BorderBrush = Brushes.LightGray;
                        if (surnameTb.BorderBrush == Brushes.Red)
                            surnameTb.BorderBrush = Brushes.LightGray;
                        if (lastNameTb.BorderBrush == Brushes.Red)
                            lastNameTb.BorderBrush = Brushes.LightGray;

                        nameTb.Text = "";
                        surnameTb.Text = "";
                        lastNameTb.Text = "";
                        loginTb.Text = "";
                        passwordTb.Text = "";
                    }
                    else
                        MessageBox.Show("В фио преподавателя не должно быть пробелов");
                }
                else
                    MessageBox.Show("Фио преподавателя должно начинаться с заглавной буквы и быть без сторонних символов");
            }
            else
            {
                if (nameTb.Text == "")
                    nameTb.BorderBrush = Brushes.Red;
                if (surnameTb.Text == "")
                    surnameTb.BorderBrush = Brushes.Red;
                if (lastNameTb.Text == "")
                    lastNameTb.BorderBrush = Brushes.Red;
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameTb.Text != "" && surnameTb.Text != "" && lastNameTb.Text != "")
            {
                if (FieldChecker.CheckTeacher(surnameTb.Text) && FieldChecker.CheckTeacher(nameTb.Text) && FieldChecker.CheckTeacher(lastNameTb.Text))
                {
                    if (FieldChecker.DeleteSpaces(surnameTb.Text) && FieldChecker.DeleteSpaces(lastNameTb.Text) && FieldChecker.DeleteSpaces(nameTb.Text))
                    {
                        if (loginTb.Text != "" && passwordTb.Text != "")
                        {
                            db.Teachers.Add(new m.Teacher { Surname = surnameTb.Text, Name = nameTb.Text, LastName = lastNameTb.Text, Login = loginTb.Text, Password = passwordTb.Text, RoleId = 1 });
                        }
                        else
                        {
                            db.Teachers.Add(new m.Teacher { Surname = surnameTb.Text, Name = nameTb.Text, LastName = lastNameTb.Text, RoleId = 1 });
                        }

                        db.SaveChanges();

                        LoadData();

                        if (nameTb.BorderBrush == Brushes.Red)
                            nameTb.BorderBrush = Brushes.LightGray;
                        if (surnameTb.BorderBrush == Brushes.Red)
                            surnameTb.BorderBrush = Brushes.LightGray;
                        if (lastNameTb.BorderBrush == Brushes.Red)
                            lastNameTb.BorderBrush = Brushes.LightGray;

                        nameTb.Text = "";
                        surnameTb.Text = "";
                        lastNameTb.Text = "";
                    }
                    else
                        MessageBox.Show("В фио преподавателя не должно быть пробелов");
                }
                else
                    MessageBox.Show("Фио преподавателя должно начинаться с заглавной буквы и быть без сторонних символов");
            }
            else
            {
                if (nameTb.Text == "")
                    nameTb.BorderBrush = Brushes.Red;
                if (surnameTb.Text == "")
                    surnameTb.BorderBrush = Brushes.Red;
                if (lastNameTb.Text == "")
                    lastNameTb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            m.Teacher item = Grid.SelectedItem as m.Teacher;

            if (item != null)
            {
                nameTb.Text = item.Name;
                surnameTb.Text = item.Surname;
                lastNameTb.Text = item.LastName;
                loginTb.Text = item.Login;
                passwordTb.Text = item.Password;
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

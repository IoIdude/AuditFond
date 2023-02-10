using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Audit.xaml
    /// </summary>
    public partial class Audit : Page
    {
        ObservableCollection<m.TeacherAudienceEl> memberData;
        ObservableCollection<m.TeacherEl> cbData;
        ApplicationContext db;
        FieldChecker FieldChecker;

        public Audit()
        {
            InitializeComponent();

            db = new ApplicationContext();
            memberData = new ObservableCollection<m.TeacherAudienceEl>();
            cbData = new ObservableCollection<m.TeacherEl>();
            FieldChecker = new FieldChecker();

            LoadData();
        }

        private void LoadData()
        {
            if (memberData != null)
                memberData.Clear();
            if (cbData != null)
                cbData.Clear();

            var data = from teacherAudiences in db.TeacherAudiences
                       join teachers in db.Teachers on teacherAudiences.TeacherId equals teachers.Id
                       select new
                       {
                           Id = teacherAudiences.Id,
                           TeacherId = teachers.Id,
                           LastName = teachers.LastName,
                           Name = teachers.Name,
                           Surname = teachers.Surname,
                           Number = teacherAudiences.Number,
                       };

            foreach (var item in data)
                memberData.Add(new m.TeacherAudienceEl
                {
                    Id = item.Id,
                    TeacherId = item.TeacherId,
                    Surname = item.Surname,
                    Name = item.Name,
                    LastName = item.LastName,
                    Number = item.Number
                });

            Grid.DataContext = memberData;


            var teacher = from teachaud in db.Teachers
                          select new
                          {
                              Id = teachaud.Id,
                              Name = teachaud.Name,
                              LastName = teachaud.LastName,
                              Surname = teachaud.Surname
                          };

            foreach (var item in teacher)
            {
                if (item.Name != "Admin")
                {
                    cbData.Add(new m.TeacherEl
                    {
                        Id = item.Id,
                        Name = item.Surname + " " + item.Name + " " + item.LastName
                    });
                }
            }

            teachCb.DisplayMemberPath = "Name";
            teachCb.SelectedValuePath = "Id";
            teachCb.ItemsSource = cbData.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Teacher());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Group());
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

            if (teachCb.SelectedIndex > -1)
                teachCb.SelectedIndex = -1;
            if (audTb.Text != "")
                audTb.Text = "";

            m.TeacherAudienceEl items = Grid.SelectedItem as m.TeacherAudienceEl;

            m.TeacherAudience cur = db.TeacherAudiences.FirstOrDefault(c => c.Id == items.Id);

            db.TeacherAudiences.Remove(cur);
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

            if (audTb.Text != "" && teachCb.SelectedIndex > -1)
            {
                if (FieldChecker.CheckAud(audTb.Text))
                {
                    m.TeacherEl selectedTeach = teachCb.SelectedItem as m.TeacherEl;
                    var curTeach = db.Teachers.FirstOrDefault(p => p.Id == selectedTeach.Id);

                    var uniqField = db.TeacherAudiences.FirstOrDefault(p => p.TeacherId == selectedTeach.Id && p.Number == int.Parse(audTb.Text));

                    if (uniqField != null)
                    {
                        MessageBox.Show("Такая запись уже существует");
                        return;
                    }

                    m.TeacherAudienceEl curAud = Grid.SelectedItem as m.TeacherAudienceEl;
                    var aud = db.TeacherAudiences.FirstOrDefault(p => p.Id == curAud.Id);

                    aud.Teacher = curTeach;
                    aud.Number = int.Parse(audTb.Text);

                    db.TeacherAudiences.Update(aud);
                    db.SaveChanges();

                    LoadData();

                    if (audTb.BorderBrush == Brushes.Red)
                        audTb.BorderBrush = Brushes.LightGray;
                    if (teachCb.BorderBrush == Brushes.Red)
                        teachCb.BorderBrush = Brushes.LightGray;

                    audTb.Text = "";
                    teachCb.SelectedIndex = -1;
                }
                else
                    MessageBox.Show("В номере аудитории должно быть число без пробелов");
            }
            else
            {
                if (audTb.Text == "")
                    audTb.BorderBrush = Brushes.Red;
                if (teachCb.SelectedIndex > -1)
                    teachCb.BorderBrush = Brushes.Red;
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (audTb.Text != "" && teachCb.SelectedIndex > -1)
            {
                if (!FieldChecker.CheckAud(audTb.Text))
                {
                    MessageBox.Show("В номере аудитории должно быть число без пробелов");
                    return;
                }

                m.TeacherEl selectedTeach = teachCb.SelectedItem as m.TeacherEl;
                var curTeach = db.Teachers.FirstOrDefault(p => p.Id == selectedTeach.Id);

                var dayCheck = db.TeacherAudiences.FirstOrDefault(p => p.TeacherId == curTeach.Id && p.Number == int.Parse(audTb.Text));

                if (dayCheck != null)
                {
                    MessageBox.Show("Такая запись уже существует");
                    return;
                }

                m.TeacherAudience aud = new m.TeacherAudience { Number = int.Parse(audTb.Text), Teacher = curTeach};
                db.TeacherAudiences.Add(aud);
                db.SaveChanges();

                LoadData();

                if (teachCb.BorderBrush == Brushes.Red)
                    teachCb.BorderBrush = Brushes.LightGray;
                if (audTb.BorderBrush == Brushes.Red)
                    audTb.BorderBrush = Brushes.LightGray;

                teachCb.SelectedIndex = -1;
                audTb.Text = "";
            }
            else
            {
                if (audTb.Text == "")
                    audTb.BorderBrush = Brushes.Red;
                if (teachCb.SelectedIndex < 0)
                    teachCb.BorderBrush = Brushes.Red;
            }
        }

        private void Grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            m.TeacherAudienceEl item = Grid.SelectedItem as m.TeacherAudienceEl;

            if (item != null)
            {
                audTb.Text = item.Number.ToString();
                teachCb.SelectedValue = item.TeacherId;
            }
            else
                return;
        }

        public static DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();
            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);
            return dt;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

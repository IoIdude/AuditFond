using System;
using System.Collections.Generic;
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
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();

            var user = db.Teachers.FirstOrDefault(p => p.Login == "Admin");

            if (user == null) 
            {
                var role = db.Roles.FirstOrDefault(x => x.Name == "Admin");

                db.Teachers.Add(new models.Teacher { Name = "Admin", Surname = "Admin", LastName = "Admin", Login = "Admin", Password = "Admin", Role = role });
                db.SaveChanges();
            }

            MainFrame.Content = new Auth();
        }
    }
}

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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        ApplicationContext db;

        public Auth()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (loginTb.Text != "" && passwordTb.Password != "")
            {
                var user = db.Teachers.FirstOrDefault(p => p.Login == loginTb.Text && p.Password == passwordTb.Password);

                if (user != null)
                {
                    var role = db.Roles.FirstOrDefault(p => p.Id == user.RoleId);

                    if (role.Name == "Admin")
                    {
                        NavigationService.Navigate(new Teacher());
                    }
                    else
                    {
                        NavigationService.Navigate(new CreateFond());
                    }
                    
                }
                else
                {
                    MessageBox.Show("Такого пользователя не существует");
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

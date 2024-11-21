using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.Windows;
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

namespace KursachIT.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    //public partial class ITAdminEntities : DbContext 
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PasswordPb.Password))
            {
                MBClass.ErrorMB("Введите пароль");
            }
            else 
            {
                try
                {
                    var user = ITAdminEntities.GetContext().User
                        .FirstOrDefault(u => u.Login == LoginTb.Text);
                    if (user == null)
                    {
                        MBClass.ErrorMB("Введен неправильный логин");
                        LoginTb.Focus();
                    }
                    else if (user.Password != PasswordPb.Password)
                    {
                        MBClass.ErrorMB("Введен неправильный пароль");
                    }
                    else
                    {
                        switch (user.IdRole)
                        {
                            case 1:
                                NavigationService.Navigate(new AdminFolder.RequestList());
                                break;
                            case 2:
                                MBClass.InformationMB("Помощник");
                                break;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }
        }
    }
}

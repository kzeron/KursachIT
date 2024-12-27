using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using KursachIT.DataFolder;
using KursachIT.ClassFolder;

namespace KursachIT.PageFolder.AddPages
{
    /// <summary>
    /// Логика взаимодействия для PageAddLogin.xaml
    /// </summary>
    public partial class PageAddLogin : Page
    {

        public PageAddLogin()
        {
            InitializeComponent();
            RoleCb.ItemsSource = ITAdminEntities.GetContext().Role.ToList(); 
        }
        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(LoginEmTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
            }
            else if (string.IsNullOrEmpty(PassowordEmTb.Password))
            {
                MBClass.ErrorMB("Введите данные пароль"); 
            }
            else if (RoleCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выбирите Уровень доступа");
            }
            else 
            {
                try
                {
                    using(var context = new ITAdminEntities())
                    {
                        var selectedRole = context.Role.FirstOrDefault(r => r.IdRole == ((DataFolder.Role)RoleCb.SelectedItem).IdRole);
                        var user = new User
                        {
                            Login = LoginEmTb.Text,
                            Password = PassowordEmTb.Password,
                            Role = selectedRole,
                        };
                        context.User.Add(user);
                        context.SaveChanges();
                        int lastUserId = user.IdLogin;

                        NavigationService.Navigate(new PageAddEmploye(lastUserId));
                    };
                }
                catch(Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}

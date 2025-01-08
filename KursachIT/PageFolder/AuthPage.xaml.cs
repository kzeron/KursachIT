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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursachIT.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
            this.Loaded += AuthPage_Loaded;
        }

        private void AuthPage_Loaded(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow == null)
            {
                MBClass.ErrorMB("Окно не найдено.");
            }
            else
            {
                var currentUser = ClassSaveSassion.LoadSession();
                if (currentUser != null)
                {
                    try
                    {
                            var employer = currentUser.Employers
                                .FirstOrDefault(u => u.IdUser == currentUser.IdLogin);
                            if (employer == null)
                            {
                                MBClass.ErrorMB("Связанный сотрудник не найден.");
                                return;
                            }
                            if (employer.IdStatus == (int)UserStatus.Fired)
                            {
                                MBClass.ErrorMB("Ваша учетная запись не действительна");
                                ClassSaveSassion.ClearSession();
                                return; // Немедленно выйти из метода
                            }
                    }
                    catch (Exception ex)
                    {
                        MBClass.ErrorMB(ex);
                    }
                    switch (currentUser.IdRole)
                    {
                        case 1:
                            MainMenu main = new MainMenu();
                            main.Show();
                            if (parentWindow != null)
                            {
                                parentWindow.Close();
                            }
                            break;
                        case 2:
                            MBClass.InformationMB("Помощник");
                            break;
                        case 3:
                            UserWindowMain user = new UserWindowMain();
                            user.Show();
                            parentWindow.Close();
                            break;
                    }
                   
                }
            }
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
                    var context = ITAdminEntities.GetContext();
                    var user = context.User
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
                        var employer = context.Employers
                            .FirstOrDefault(u => u.IdUser == user.IdLogin);
                        if (employer == null)
                        {
                            MBClass.ErrorMB("Связанный сотрудник не найден.");
                            return;
                        }
                        if (employer.IdStatus == (int)UserStatus.Fired)
                        {
                            MBClass.ErrorMB("Ваша учетная запись не действительна");
                            return; // Немедленно выйти из метода
                        }
                        ClassSaveSassion.SaveSession(user);

                        switch (user.IdRole)
                        {
                            case 1:
                                MainMenu main = new MainMenu();
                                main.Show();
                                Window.GetWindow(this).Close();
                                break;
                            case 2:
                                MBClass.InformationMB("Помощник");
                                break;
                            case 3:
                                UserWindowMain userWin = new UserWindowMain();
                                userWin.Show();
                                Window.GetWindow(this).Close();
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

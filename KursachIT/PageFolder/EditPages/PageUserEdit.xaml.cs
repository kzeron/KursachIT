using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KursachIT.ClassFolder;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KursachIT.DataFolder;
using KursachIT.Windows;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageUserEdit.xaml
    /// </summary>
    public partial class PageUserEdit : Page
    {
        private ClassUser _selectedUser;

        public PageUserEdit(ClassUser selectedUser)
        {
            InitializeComponent();
            _selectedUser = selectedUser;
            RoleCb.ItemsSource = ITAdminEntities.GetContext().Role.ToList();
            DataContext = this;
        }

        public System.Collections.ObjectModel.ObservableCollection<DataFolder.Role> Roles =>
            new System.Collections.ObjectModel.ObservableCollection<DataFolder.Role>(ITAdminEntities.GetContext().Role.ToList());

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginEmTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
            }
            else if (string.IsNullOrEmpty(PassowordEmTb.Password))
            {
                MBClass.ErrorMB("Введите пароль");
            }
            else if (RoleCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите уровень доступа");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        var user = context.User.FirstOrDefault(u => u.IdLogin == _selectedUser.IdUser);
                        if (user != null)
                        {
                            user.Login = LoginEmTb.Text;
                            user.Password = PassowordEmTb.Password;
                            user.Role = context.Role.FirstOrDefault(r => r.IdRole == ((DataFolder.Role)RoleCb.SelectedItem).IdRole);
                            context.SaveChanges();
                        }
                    }
                    MBClass.InformationMB("Данные успешно сохранены");
                    ((AnketWin)Window.GetWindow(this))?.Close();
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex.Message);
                }
            }
        }
        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}

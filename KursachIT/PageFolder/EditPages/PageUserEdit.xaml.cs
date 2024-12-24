using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KursachIT.ClassFolder;
using KursachIT.DataFolder;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageUserEdit.xaml
    /// </summary>
    public partial class PageUserEdit : Page
    {
        User _currentUser;
        public int UserId { get; set; }

        public PageUserEdit(int idUser)
        {
            InitializeComponent();
            this.UserId = idUser;
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = ITAdminEntities.GetContext().User
             .FirstOrDefault(i => i.IdLogin == this.UserId);

            _currentUser = user;
            DataContext = user;

            LoadRole();
            RoleCb.SelectedValue = user.IdRole;
            LoginEmTb.Text = user.Login;
            PassowordEmTb.Password = user.Password;
        }

        private void LoadRole()
        {
            var role = ITAdminEntities.GetContext().Role.ToList(); // Предполагается, что у вас есть таблица Suppliers
            RoleCb.ItemsSource = role;
            RoleCb.DisplayMemberPath = "NameRole"; // Убедитесь, что это поле существует
            RoleCb.SelectedValuePath = "IdRole";
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginEmTb.Text))
            {
                MBClass.ErrorMB("Введите логин.");
                return;
            }

            if (string.IsNullOrEmpty(PassowordEmTb.Password))
            {
                MBClass.ErrorMB("Введите пароль.");
                return;
            }

            if (RoleCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите роль.");
                return;
            }

            try
            {
                _currentUser.Login = LoginEmTb.Text;
                _currentUser.Password = PassowordEmTb.Password;
                if (RoleCb.SelectedItem is DataFolder.Role selectedRole)
                {
                    _currentUser.IdRole = selectedRole.IdRole;
                }

                ITAdminEntities.GetContext().SaveChanges();
                MBClass.InformationMB("Данные успешно сохранены.");
                NavigationService.Navigate(new PageEditEmployer(_currentUser.IdLogin));
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка: {ex.Message}");
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close(); 
        }
    }
}

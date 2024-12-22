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
        private readonly int _idUser;
        private User _currentUser;

        public PageUserEdit(int idUser)
        {
            InitializeComponent();
            _idUser = idUser;
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (var context = ITAdminEntities.GetContext())
            {
                _currentUser = context.User.FirstOrDefault(u => u.IdLogin == _idUser);

                if (_currentUser == null)
                {
                    MBClass.ErrorMB("Пользователь не найден.");
                    return;
                }

                LoginEmTb.Text = _currentUser.Login;
                PassowordEmTb.Password = _currentUser.Password;

                RoleCb.ItemsSource = context.Role.ToList();
                RoleCb.DisplayMemberPath = "NameRole";
                RoleCb.SelectedValuePath = "IdRole";
                RoleCb.SelectedValue = _currentUser.Role?.IdRole;
            }
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
                using (var context = ITAdminEntities.GetContext())
                {
                    var user = context.User.FirstOrDefault(u => u.IdLogin == _idUser);

                    if (user != null)
                    {
                        user.Login = LoginEmTb.Text;
                        user.Password = PassowordEmTb.Password;
                        user.IdRole = (int)RoleCb.SelectedValue;

                        context.SaveChanges();
                        MBClass.InformationMB("Данные успешно сохранены.");
                    }
                }
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

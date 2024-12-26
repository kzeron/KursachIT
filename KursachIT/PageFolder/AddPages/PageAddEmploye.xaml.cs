using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KursachIT.PageFolder.AddPages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEmploye.xaml
    /// </summary>
    public partial class PageAddEmploye : Page
    {
        private string _photoPath;
        int _newIdUser;
        public PageAddEmploye(int userId)
        {
            InitializeComponent();
            _newIdUser = userId;
            LoadOffice();
        }
        private void LoadOffice()
        {
            using (var context = new ITAdminEntities())
            {
                var offices = context.Office.ToList();
                var numbers = context.Cabinet.ToList();
                NameOfficeCb.ItemsSource = offices;
                NamberOfficeCb.ItemsSource = numbers;
                
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEmTb.Text))
            {
                MBClass.ErrorMB("Введите имя");
            }
            else if (string.IsNullOrWhiteSpace(SurnameEmTb.Text))
            {
                MBClass.ErrorMB("Введите Фамилию");
            }
            else if (NamberOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберете кабинет");
            }
            else if (NameOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберете кабинет");
            }
            else if (string.IsNullOrWhiteSpace(EmailEmTb.Text))
            {
                MBClass.ErrorMB("Введите почту");
            }
            else if (string.IsNullOrWhiteSpace(PhoneEmTb.Text))
            {
                MBClass.ErrorMB("Укажите телефон");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        var selectedOfficeId = ((Office)NameOfficeCb.SelectedItem).IdOffice;
                        var selectedCabinetId = ((Cabinet)NamberOfficeCb.SelectedItem).IdNumberCabinet;

                        // Создаем нового сотрудника
                        var employer = new Employers
                        {
                            Name = NameEmTb.Text,
                            Lastname = SurnameEmTb.Text,
                            Patronymic = PathronymicEmTb.Text,
                            numberPhone = PhoneEmTb.Text,
                            email = EmailEmTb.Text,
                            IdOffice = selectedOfficeId, // Передаем ID отдела
                            IdCab = selectedCabinetId, // Передаем ID кабинета
                            IdUser = _newIdUser,
                            IdStatus = 7,
                            PhotoPath = _photoPath // Сохраняем путь к фотографии
                        };

                        context.Employers.Add(employer);
                        context.SaveChanges();
                    }
                    Window.GetWindow(this).Close();
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex.Message);
                }
            }

        }
        public static class SharedData
        {
            public static int LastUserId { get; set; }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            
            using (var context = new ITAdminEntities())
            {
                // Проверяем, связан ли логин с сотрудником
                var userHasEmployee = context.Employers.Any(emp => emp.IdUser == _newIdUser);

                if (!userHasEmployee)
                {
                    // Удаляем логин, если сотрудник не был создан
                    var userToDelete = context.User.Find(_newIdUser);
                    if (userToDelete != null)
                    {
                        context.User.Remove(userToDelete);
                        context.SaveChanges();
                        MBClass.InformationMB("Логин удалён, так как сотрудник не был добавлен.");
                    }
                }
            }

            // Закрываем окно
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
        private void PhotoAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif",
                Title = "Выберите фотографию"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Сохраняем выбранный путь к изображению
                var selectedImagePath = openFileDialog.FileName;

                try
                {
                    // Загружаем изображение в интерфейс
                    EmployeePhoto.Source = new BitmapImage(new Uri(selectedImagePath));

                    // Сохраняем путь в переменную для дальнейшей обработки
                    _photoPath = selectedImagePath;
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB($"Ошибка загрузки фотографии: {ex.Message}");
                }
            }
        }


    }
}

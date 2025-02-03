using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageEditEmployer.xaml
    /// </summary>
    public partial class PageEditEmployer : Page
    {
        private string _photoPath;
        private readonly int _idUser;
        private Employers _currentEmployer;

        public PageEditEmployer(int idUser)
        {
            InitializeComponent();
            _idUser = idUser;
           LoadEmployeeData();
        }
        private void LoadCabinets()
        {
            var numberofiice = ITAdminEntities.GetContext().Cabinet.ToList();
            NamberOfficeCb.ItemsSource = numberofiice;
            NamberOfficeCb.DisplayMemberPath = "NumberCabinet";
            NamberOfficeCb.SelectedValuePath = "IdNumberCabinet";
        }

        private void LoadOffice()
        {
            var nameOfice = ITAdminEntities.GetContext().Office.ToList();
            NameOfficeCb.ItemsSource = nameOfice;
            NameOfficeCb.DisplayMemberPath = "NameOffice";
            NameOfficeCb.SelectedValuePath = "IdOffice";

        }
        private void LoadEmployeeData()
        {
            using (var context = new ITAdminEntities())
            {
                _currentEmployer = context.Employers.FirstOrDefault(emp => emp.IdUser == _idUser);

                if (_currentEmployer != null)
                {
                    if (_currentEmployer.Photo != null && _currentEmployer.Photo.Length > 0)
                    {
                        EmployeePhoto.Source = ByteArrayToImage(_currentEmployer.Photo);
                    }
                    NameEmTb.Text = _currentEmployer.Name;
                    SurnameEmTb.Text = _currentEmployer.Lastname;
                    PathronymicEmTb.Text = _currentEmployer.Patronymic;
                    PhoneEmTb.Text = _currentEmployer.numberPhone;
                    EmailEmTb.Text = _currentEmployer.email;

                    LoadOffice();
                    LoadCabinets();
                    NameOfficeCb.SelectedValue = _currentEmployer.IdOffice;
                    NamberOfficeCb.SelectedValue = _currentEmployer.IdCab;
                }
                else
                {
                    // Handle the case where the employee is not found
                    MBClass.ErrorMB("Сотрудник не найден.");
                }
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            // Проверка ввода данных
            if (string.IsNullOrWhiteSpace(NameEmTb.Text))
            {
                MBClass.ErrorMB("Введите имя");
                return;
            }
            if (string.IsNullOrWhiteSpace(SurnameEmTb.Text))
            {
                MBClass.ErrorMB("Введите фамилию");
                return;
            }
            if (NamberOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите кабинет");
                return;
            }
            if (NameOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите офис");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailEmTb.Text))
            {
                MBClass.ErrorMB("Введите почту");
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneEmTb.Text))
            {
                MBClass.ErrorMB("Введите телефон");
                return;
            }
            else if (!ClassDataValidator.IsEmailValid(EmailEmTb.Text))
            {
                MBClass.ErrorMB("Некорректный формат почты или недопустимый домен (допустимы: gmail.com, yandex.ru, mail.ru).");
            }
            else if (!ClassDataValidator.IsPhoneNumberValid(PhoneEmTb.Text))
            {
                MBClass.ErrorMB("Телефон должен содержать только цифры и, возможно, знак '+' в начале.");
            }
            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Попытка найти сотрудника для редактирования
                    var employer = context.Employers.FirstOrDefault(emp => emp.IdUser == _idUser);

                    if (employer != null)
                    {
                        employer.Name = NameEmTb.Text;
                        employer.Lastname = SurnameEmTb.Text;
                        employer.Patronymic = PathronymicEmTb.Text;
                        employer.numberPhone = PhoneEmTb.Text;
                        employer.email = EmailEmTb.Text;
                        employer.IdOffice = (int)NameOfficeCb.SelectedValue;
                        employer.IdCab = (int)NamberOfficeCb.SelectedValue;
                        employer.IdStatus = 7;

                        if (!string.IsNullOrEmpty(_photoPath))
                        {
                            employer.Photo = ImageToByteArray(_photoPath);
                        }

                        context.SaveChanges();
                        MBClass.InformationMB("Данные сотрудника успешно обновлены.");
                        Window.GetWindow(this)?.Close();
                    }
                    else
                    {
                        // Если сотрудник не найден, выводим ошибку
                        MBClass.ErrorMB("Сотрудник не найден. Возможно, данные устарели.");
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

            NavigationService.GoBack();
        }
        private byte[] ImageToByteArray(string imagePath)
        {
            return File.ReadAllBytes(imagePath);
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
                _photoPath = openFileDialog.FileName;

                try
                {
                    // Загружаем изображение в интерфейс
                    EmployeePhoto.Source = new BitmapImage(new Uri(_photoPath));
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB($"Ошибка загрузки фотографии: {ex.Message}");
                }
            }
        }
        private BitmapImage ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }


    }
}


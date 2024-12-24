using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageEditEmployer.xaml
    /// </summary>
    public partial class PageEditEmployer : Page
    {
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

            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Попытка найти сотрудника для редактирования
                    var employer = context.Employers.FirstOrDefault(emp => emp.IdUser == _idUser);

                    if (employer != null)
                    {
                        // Обновление данных сотрудника
                        employer.Name = NameEmTb.Text;
                        employer.Lastname = SurnameEmTb.Text;
                        employer.Patronymic = PathronymicEmTb.Text;
                        employer.numberPhone = PhoneEmTb.Text;
                        employer.email = EmailEmTb.Text;
                        employer.IdOffice = (int)NameOfficeCb.SelectedValue;
                        employer.IdCab = (int)NamberOfficeCb.SelectedValue;
                        employer.IdStatus = 7;

                        // Сохранение изменений
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
    }
}


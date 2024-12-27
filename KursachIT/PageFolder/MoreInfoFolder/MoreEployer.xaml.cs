using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using KursachIT.ClassFolder;
using KursachIT.DataFolder;

namespace KursachIT.PageFolder.MoreFolder
{
    public partial class MoreEployer : Page
    {
        private readonly int _selectedEmployer;

        public MoreEployer(int employer)
        {
            InitializeComponent();
            _selectedEmployer = employer;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Поиск данных сотрудника по ID
                    var employer = context.Employers.FirstOrDefault(emp => emp.IdEmployers == _selectedEmployer);

                    if (employer != null)
                    {
                        // Заполнение полей интерфейса
                        NameTextBlock.Text = employer.Name;
                        LastNameTextBlock.Text = employer.Lastname;
                        PatronymicTextBlock.Text = employer.Patronymic;
                        NumberPhoneTextBlock.Text = employer.numberPhone;
                        EmailTextBlock.Text = employer.email;

                        var office = context.Office.FirstOrDefault(o => o.IdOffice == employer.IdOffice);
                        NameOfficeTextBlock.Text = office?.NameOffice ?? "Не указан";

                        var cabinet = context.Cabinet.FirstOrDefault(c => c.IdNumberCabinet == employer.IdCab);
                        NumberOfficeTextBlock.Text = cabinet?.NumberCabinet.ToString() ?? "Не указан";

                        // Загрузка изображения
                        if (!string.IsNullOrEmpty(employer.PhotoPath))
                        {
                            PhotoImage.Source = new BitmapImage(new Uri(employer.PhotoPath, UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            PhotoImage.Source = null; // Или установить изображение-заглушку
                        }
                    }
                    else
                    {
                        MBClass.ErrorMB("Сотрудник не найден.");
                        NavigationService.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close(); 
        }
    }
}

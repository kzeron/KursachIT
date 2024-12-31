using KursachIT.ClassFolder;
using KursachIT.DataFolder;
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
    /// Логика взаимодействия для PersAcc.xaml
    /// </summary>
    public partial class PersAcc : Page
    {
        public PersAcc()
        {
            InitializeComponent();
            LoadCurrentEmployeeData();
        }

        private void LoadCurrentEmployeeData()
        {
            try
            {
                var currentUser = ClassSaveSassion.LoadSession();
                if (currentUser == null)
                {
                    MBClass.ErrorMB("Сессия пользователя не найдена. Проверьте вход в систему.");
                    NavigationService.GoBack();
                    return;
                }

                using (var context = new ITAdminEntities())
                {
                    // Поиск сотрудника, связанного с текущим пользователем
                    var employer = context.Employers.FirstOrDefault(emp => emp.IdUser == currentUser.IdLogin);

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
                        MBClass.ErrorMB("Информация о сотруднике не найдена.");
                        NavigationService.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка загрузки данных: {ex.Message}");
            }
        }
    }
}

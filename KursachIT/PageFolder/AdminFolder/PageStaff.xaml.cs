using KursachIT.PageFolder.MoreFolder;
using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.PageFolder.AddPages;
using KursachIT.PageFolder.EditPages;
using KursachIT.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace KursachIT.PageFolder.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для PageStaff.xaml
    /// </summary>
    public partial class PageStaff : Page
    {
        private ObservableCollection<ClassUser> _users;
        public PageStaff()
        { 
                InitializeComponent();
                _users = new ObservableCollection<ClassUser>();
                LoadData();
                StaffDgList.ItemsSource = _users;
        }
        private void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                var staffData = (from Employers in context.Employers
                                 join User in context.User on Employers.IdUser equals User.IdLogin
                                 join Role in context.Role on User.IdRole equals Role.IdRole into UserGroup
                                 from Role in UserGroup.DefaultIfEmpty()
                                 join Office in context.Office on Employers.IdOffice equals Office.IdOffice
                                 join Cabinet in context.Cabinet on Employers.IdCab equals Cabinet.IdNumberCab into OfficeGroup
                                 from Cabinet in OfficeGroup.DefaultIfEmpty()
                                 join Status in context.Status on Employers.IdStatus equals Status.IdStatus into StatusGroup
                                 from Status in StatusGroup.DefaultIfEmpty()
                                 select new
                                 {
                                     Employers.IdEmployers,
                                     User.IdLogin,
                                     Role.NameRole,
                                     Employers.Name,
                                     Employers.Lastname,
                                     Employers.Patronymic,
                                     Office.NameOffice,
                                     Cabinet.numberCab,
                                     Employers.email,
                                     Employers.numberPhone,
                                     Status.NameStatus
                                 }).OrderBy(u => u.IdLogin)
                                 .ToList();

                _users.Clear();
                foreach (var user in staffData)
                {
                    _users.Add(new ClassUser
                    {
                        IdUser = user.IdLogin,
                        IdEmployers = user.IdEmployers,
                        Name = user.Name,
                        LastName = user.Lastname,
                        Patronymic = user.Patronymic,
                        NameOffice = user.NameOffice,
                        NumberOffice = user.numberCab,
                        Email = user.email,
                        NumberPhone = user.numberPhone
                    });
                }
                StaffDgList.ItemsSource = _users;
            }
        }



        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            AnketWin anketWin = new AnketWin(new PageAddLogin());
            anketWin.Show();

            LoadData();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDgList.SelectedItem is ClassUser selectedUser)
            {
                // Создаем экземпляр страницы с деталями и передаем данные
                PageUserEdit userDetailsPage = new PageUserEdit(selectedUser.IdUser);

                // Открываем новую страницу в окне
                AnketWin detailsWindow = new AnketWin(userDetailsPage);
                detailsWindow.Show();
            }
            else
            {
                MBClass.ErrorMB("Выберите сотрудника для просмотра.");
            }
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (StaffDgList.SelectedItem is ClassUser selectedUser)
            {
                bool result = MBClass.QuestionMB($"Вы уверены, что хотите изменить статус сотрудника {selectedUser.Name} {selectedUser.LastName} на 'Удален'?");

                if (result)
                {
                    try
                    {
                        using (var context = new ITAdminEntities())
                        {
                            // Найти запись сотрудника в таблице Employers
                            var employerToUpdate = context.Employers.FirstOrDefault(emp => emp.IdEmployers == selectedUser.IdEmployers);

                            if (employerToUpdate != null)
                            {
                                // Изменить статус сотрудника
                                employerToUpdate.IdStatus = 8;

                                // Сохранить изменения
                                context.SaveChanges();

                                MBClass.InformationMB("Статус сотрудника успешно изменен.");
                                LoadData(); // Обновить данные после изменения
                            }
                            else
                            {
                                MBClass.ErrorMB("Сотрудник не найден.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MBClass.ErrorMB($"Произошла ошибка при изменении статуса: {ex.Message}");
                    }
                }
            }
            else
            {
                MBClass.ErrorMB("Выберите сотрудника для изменения статуса.");
            }
        }


        private void StaffDgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StaffDgList.SelectedItem is ClassUser selectedUser)
            {
                // Создаем экземпляр страницы с деталями и передаем данные
                MoreEployer userDetailsPage = new MoreEployer(selectedUser);

                // Открываем новую страницу в окне
                AnketWin detailsWindow = new AnketWin(userDetailsPage);
                detailsWindow.Show();
            }
            else
            {
                MBClass.ErrorMB("Выберите сотрудника для просмотра.");
            }
        }
        private void OpenFilterPopup_Click(object sender, RoutedEventArgs e)
        {
            // Открытие/закрытие Popup
            FilterPopup.IsOpen = !FilterPopup.IsOpen;
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            // Сбор данных фильтров
            var selectedDepartments = new List<string>();
            if (ITDepartmentCheckBox.IsChecked == true) selectedDepartments.Add("ИТ");
            if (TechSupportCheckBox.IsChecked == true) selectedDepartments.Add("Техническая поддержка");
            if (FinanceCheckBox.IsChecked == true) selectedDepartments.Add("Бухгалтерия");
            if (MarketingCheckBox.IsChecked == true) selectedDepartments.Add("Маркетинг");
            if (SalesDepartmentCheckBox.IsChecked == true) selectedDepartments.Add("Отдел продаж");

            var searchText = SearchTextBox.Text;

            // Применение фильтра
            ApplyFilters(selectedDepartments, searchText);

            // Закрытие Popup
            FilterPopup.IsOpen = false;
        }

        private void ApplyFilters(List<string> departments, string searchText)
        {
            using (var context = new ITAdminEntities())
            {
                var query = from Employers in context.Employers
                            join User in context.User on Employers.IdUser equals User.IdLogin
                            join Role in context.Role on User.IdRole equals Role.IdRole into UserGroup
                            from Role in UserGroup.DefaultIfEmpty()
                            join Office in context.Office on Employers.IdOffice equals Office.IdOffice
                            join Cabinet in context.Cabinet on Employers.IdCab equals Cabinet.IdNumberCab into OfficeGroup
                            from Cabinet in OfficeGroup.DefaultIfEmpty()
                            select new
                            {
                                Employers.IdEmployers,
                                User.IdLogin,
                                Role.NameRole,
                                Employers.Name,
                                Employers.Lastname,
                                Employers.Patronymic,
                                Office.NameOffice,
                                Cabinet.numberCab,
                                Employers.numberPhone,
                                Employers.email
                            };

                // Применение фильтрации по отделам
                if (departments.Any())
                {
                    query = query.Where(u => departments.Contains(u.NameOffice));
                }

                // Применение фильтрации по тексту
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(u => u.Name.Contains(searchText) ||
                                             u.Lastname.Contains(searchText) ||
                                             u.Patronymic.Contains(searchText) ||
                                             u.NameOffice.Contains(searchText));
                }

                // Обновление данных DataGrid
                var filteredData = query.OrderBy(u => u.IdLogin).ToList();
                _users.Clear();
                foreach (var user in filteredData)
                {
                    _users.Add(new ClassUser
                    {
                        IdUser = user.IdLogin,
                        IdEmployers = user.IdEmployers,
                        Name = user.Name,
                        LastName = user.Lastname,
                        Patronymic = user.Patronymic,
                        NameOffice = user.NameOffice,
                        NumberOffice = user.numberCab,
                        Email = user.email,
                        NumberPhone = user.numberPhone
                    });
                }

                StaffDgList.ItemsSource = _users;
            }

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text;
            ApplyFilters(new List<string>(), searchText);
        }
    }
}

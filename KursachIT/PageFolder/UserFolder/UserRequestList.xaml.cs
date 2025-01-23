using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.PageFolder.AddPages;
using KursachIT.PageFolder.EditPages;
using KursachIT.PageFolder.MoreFolder;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursachIT.PageFolder.UserFolder
{
    /// <summary>
    /// Логика взаимодействия для UserRequestList.xaml
    /// </summary>
    public partial class UserRequestList : Page
    {
        private ObservableCollection<ClassRequest> ModelRequest;
        public UserRequestList()
        {
            InitializeComponent();
            ModelRequest = new ObservableCollection<ClassRequest>();
            LoadData();
        }
        public void LoadData()
        {
            RequestHelper.UpdateOverdueRequests();

            var currentUser = ClassSaveSassion.LoadSession();
            if (currentUser == null)
            {
                MBClass.ErrorMB("Не удалось получить данные текущей сессии.");
                return;
            }

            using (var context = new ITAdminEntities())
            {
                // Получаем IdEmployers через IdUser
                var currentEmployer = context.Employers.FirstOrDefault(emp => emp.IdUser == currentUser.IdLogin);
                if (currentEmployer == null)
                {
                    MBClass.ErrorMB("Не удалось определить сотрудника для текущего пользователя.");
                    return;
                }

                var requestsData = (from Requests in context.Requests
                                    join Status in context.Status on Requests.IdStatus equals Status.IdStatus into StatusGroup
                                    from Status in StatusGroup.DefaultIfEmpty()
                                    join Category in context.Category on Requests.IdCategory equals Category.IdCategory into CategoryGroup
                                    from Category in CategoryGroup.DefaultIfEmpty()
                                    join Priority in context.Priority on Requests.IdPriority equals Priority.IdPriority into PriorityGroup
                                    from Priority in PriorityGroup.DefaultIfEmpty()
                                    where Requests.IdRequestSender == currentEmployer.IdEmployers // Проверка связи с текущим сотрудником
                                    select new
                                    {
                                        Requests.IdRequest,
                                        Status.NameStatus,
                                        Category.NameCategory,
                                        Priority.NamePriority,
                                        Requests.PlanDate,
                                    }).OrderBy(u => u.IdRequest)
                                    .ToList();

                ModelRequest.Clear();
                foreach (var request in requestsData)
                {
                    ModelRequest.Add(new ClassRequest
                    {
                        IdRequst = request.IdRequest,
                        NameStatus = request.NameStatus,
                        NameCategory = request.NameCategory,
                        NamePriority = request.NamePriority,
                        PlanDate = request.PlanDate,
                    });
                }
                ReqestDgList.ItemsSource = ModelRequest;
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
            var selectedPriorities = new List<string>();
            if (HighPriorityCheckBox.IsChecked == true) selectedPriorities.Add("Высокий");
            if (MediumPriorityCheckBox.IsChecked == true) selectedPriorities.Add("Средний");
            if (LowPriorityCheckBox.IsChecked == true) selectedPriorities.Add("Низкий");

            var selectedStatuses = new List<string>();
            if (PendingStatusCheckBox.IsChecked == true) selectedStatuses.Add("На рассмотрении");
            if (CompletedStatusCheckBox.IsChecked == true) selectedStatuses.Add("Выполнено");
            if (RejectedStatusCheckBox.IsChecked == true) selectedStatuses.Add("Отклонено");
            if (CancelledStatusCheckBox.IsChecked == true) selectedStatuses.Add("Отменено");
            if (OverdueStatusCheckBox.IsChecked == true) selectedStatuses.Add("Просрочено");
            if (InProgressStatusCheckBox.IsChecked == true) selectedStatuses.Add("Выполняется");

            var searchText = SearchTextBox.Text;

            // Применение фильтра
            ApplyFilters(selectedPriorities, selectedStatuses, searchText);

            // Закрытие Popup
            FilterPopup.IsOpen = false;
        }

        private void ApplyFilters(List<string> priorities, List<string> statuses, string searchText)
        {
            try
            {
                var currentUser = ClassSaveSassion.LoadSession();
                if (currentUser == null)
                {
                    MBClass.ErrorMB("Не удалось получить данные текущей сессии.");
                    return;
                }

                using (var context = new ITAdminEntities())
                {
                    // Получаем IdEmployers через IdUser
                    var currentEmployer = context.Employers.FirstOrDefault(emp => emp.IdUser == currentUser.IdLogin);
                    if (currentEmployer == null)
                    {
                        MBClass.ErrorMB("Не удалось определить сотрудника для текущего пользователя.");
                        return;
                    }

                    var query = from request in context.Requests
                                join status in context.Status on request.IdStatus equals status.IdStatus into statusGroup
                                from status in statusGroup.DefaultIfEmpty()
                                join category in context.Category on request.IdCategory equals category.IdCategory into categoryGroup
                                from category in categoryGroup.DefaultIfEmpty()
                                join priority in context.Priority on request.IdPriority equals priority.IdPriority into priorityGroup
                                from priority in priorityGroup.DefaultIfEmpty()
                                where request.IdRequestSender == currentEmployer.IdEmployers // Проверка связи с текущим сотрудником
                                select new
                                {
                                    request.IdRequest,
                                    status.NameStatus,
                                    category.NameCategory,
                                    priority.NamePriority,
                                    request.PlanDate,
                                };

                    // Применение фильтров
                    if (priorities.Any())
                    {
                        query = query.Where(r => priorities.Contains(r.NamePriority));
                    }

                    if (statuses.Any())
                    {
                        query = query.Where(r => statuses.Contains(r.NameStatus));
                    }

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query = query.Where(r => r.NameCategory.Contains(searchText) ||
                                                 r.NameStatus.Contains(searchText) ||
                                                 r.NamePriority.Contains(searchText));
                    }

                    var filteredData = query.OrderBy(r => r.IdRequest).ToList();

                    // Обновление DataGrid
                    ReqestDgList.ItemsSource = filteredData;
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            AnketWin anketWin = new AnketWin(new PageRequestAdd());
            anketWin.Show();

            LoadData();
        }

        private void ReqestDgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(ReqestDgList.SelectedItem is ClassRequest selectedRequest)
            {
                // Создаем экземпляр страницы с деталями и передаем данные
                PageRequestMore requestMore = new PageRequestMore(selectedRequest.IdRequst);

                // Открываем новую страницу в окне
                AnketWin detailsWindow = new AnketWin(requestMore);
                detailsWindow.Show();
            }
            else
            {
                MBClass.ErrorMB("Выберите сотрудника для просмотра.");
            }
        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (ReqestDgList.SelectedItem is ClassRequest selectedRequest)
            {
                if(selectedRequest.IdStatus != (int)RequestHelper.StatusEnum.Canceled && selectedRequest.IdStatus  != (int)RequestHelper.StatusEnum.Completed && selectedRequest.IdStatus != (int)RequestHelper.StatusEnum.Denied)
                {
                    MBClass.ErrorMB("Редактирование заявки невозможно");
                    return;
                }
                // Создаем экземпляр страницы с деталями и передаем данные
                EditRequest editRequest = new EditRequest(selectedRequest.IdRequst);

                // Открываем новую страницу в окне
                AnketWin detailsWindow = new AnketWin(editRequest);
                detailsWindow.Show();
            }
            else
            {
                MBClass.ErrorMB("Выберите заявку для просмотра.");
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchedText = SearchTextBox.Text;
            ApplyFilters(new List<string>(), new List<string>(), searchedText);

        }

        private void CancelRequest_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную заявку из списка
            var selectedRequest = ReqestDgList.SelectedItem as ClassRequest;
            if (selectedRequest == null)
            {
                MBClass.ErrorMB("Выберите заявку");
                return;
            }
            else
            {
                using (var context = new ITAdminEntities())
                {
                    // Ищем заявку в базе данных по Id
                    var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                    if (request != null)
                    {
                        // Устанавливаем статус "Завершено" и дату завершения
                        request.IdStatus = (int)RequestHelper.StatusEnum.Canceled;
                        request.DateRealize = DateTime.Now;

                        context.SaveChanges(); // Сохраняем изменения в базе данных
                        LoadData(); // Обновляем список заявок

                        MBClass.InformationMB("Заявка успешно завершена.");
                    }
                    else
                    {
                        MBClass.ErrorMB("Заявка не найдена.");
                    }
                }
            }
        }
    }
}

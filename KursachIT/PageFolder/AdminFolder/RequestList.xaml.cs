﻿using System;
using System.Collections.Generic;
using System.Linq;
using KursachIT.ClassFolder;
using System.Windows;
using System.Windows.Controls;
using KursachIT.DataFolder;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using KursachIT.PageFolder.MoreFolder;
using KursachIT.Windows;

namespace KursachIT.PageFolder.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для RequestList.xaml
    /// </summary>
    public partial class RequestList : Page
    {
        
        private ObservableCollection<ClassRequest> ModelRequest;

        public RequestList()
        {
            InitializeComponent();
            ModelRequest = new ObservableCollection<ClassRequest>();
            LoadData();

        }
        public void LoadData()
        {
            RequestHelper.UpdateOverdueRequests();

            using (var context = new ITAdminEntities())
            {
                var requestsData = (from Requests in context.Requests
                                    join Status in context.Status on Requests.IdStatus equals Status.IdStatus into StatusGroup
                                    from Status in StatusGroup.DefaultIfEmpty()
                                    join Category in context.Category on Requests.IdCategory equals Category.IdCategory into CategoryGroup
                                    from Category in CategoryGroup.DefaultIfEmpty()
                                    join Priority in context.Priority on Requests.IdPriority equals Priority.IdPriority into PriorityGroup
                                    from Priority in PriorityGroup.DefaultIfEmpty()
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
                        IdStatus = context.Status.FirstOrDefault(s => s.NameStatus == request.NameStatus)?.IdStatus ?? 0
                    });

                }
                ReqestDgList.ItemsSource = ModelRequest;
            }
        }


        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new PageStaff());
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = ReqestDgList.SelectedItem as ClassRequest;
            if (selectedRequest == null)
            {
                MBClass.ErrorMB("Выберите заявку");
                return;
            }
            if (selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Denied ||
                selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Canceled ||
                selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Overdue)
            {
                MBClass.ErrorMB("Заявка не действительна.");
                return;
            }


            // Получение текущего пользователя из сессии
            var session = ClassSaveSassion.LoadSession();
            if (session == null)
            {
                MBClass.ErrorMB("Сессия не найдена.");
                return;
            }

            using (var context = new ITAdminEntities())
            {
                // Поиск пользователя по идентификатору из сессии
                var user = context.User
                    .FirstOrDefault(u => u.IdLogin == session.IdLogin);

                if (user == null)
                {
                    MBClass.ErrorMB("Пользователь не найден.");
                    return;
                }

                // Поиск связанного сотрудника
                var employer = context.Employers
                    .FirstOrDefault(emp => emp.IdUser == user.IdLogin);

                if (employer == null)
                {
                    MBClass.ErrorMB("Связанный сотрудник не найден.");
                    return;
                }

                // Проверка наличия заявки
                var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                if (request != null)
                {
                    request.IdStatus = (int)RequestHelper.StatusEnum.InProgress;
                    request.IdExcutor = employer.IdEmployers; // Назначение текущего сотрудника исполнителем
                    context.SaveChanges();
                    LoadData(); // Обновление данных
                    MBClass.InformationMB("Заявка успешно принята.");
                }
                else
                {
                    MBClass.ErrorMB("Заявка не найдена или не актуальна.");
                }
            }
        }

        private void CompliteRequest_Click(object sender, RoutedEventArgs e)
        {

            // Получаем выбранную заявку из списка
            var selectedRequest = ReqestDgList.SelectedItem as ClassRequest;
            if (selectedRequest == null)
            {
                MBClass.ErrorMB("Выберите заявку");
                return;
            }
            if (selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Denied ||
                selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Canceled ||
                selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Overdue)
            {
                MBClass.ErrorMB("Заявка не действительна.");
                return;
            }


            // Получение текущего пользователя из сессии
            var session = ClassSaveSassion.LoadSession();
            if (session == null)
            {
                MBClass.ErrorMB("Сессия не найдена.");
                return;
            }

            using (var context = new ITAdminEntities())
            {
                // Поиск пользователя по идентификатору из сессии
                var user = context.User
                    .FirstOrDefault(u => u.IdLogin == session.IdLogin);

                if (user == null)
                {
                    MBClass.ErrorMB("Пользователь не найден.");
                    return;
                }

                // Поиск связанного сотрудника
                var employer = context.Employers
                    .FirstOrDefault(emp => emp.IdUser == user.IdLogin);

                if (employer == null)
                {
                    MBClass.ErrorMB("Связанный сотрудник не найден.");
                    return;
                }

                // Поиск заявки
                var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                if (request != null)
                {
                    // Проверка, является ли текущий сотрудник исполнителем заявки
                    if (request.IdExcutor != employer.IdEmployers)
                    {
                        MBClass.ErrorMB("Вы не являетесь исполнителем этой заявки.");
                        return;
                    }

                    // Устанавливаем статус "Завершено" и дату завершения
                    request.IdStatus = (int)RequestHelper.StatusEnum.Completed;
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
            using (var context = new ITAdminEntities())
            {
                var query = from Requests in context.Requests
                            join Status in context.Status on Requests.IdStatus equals Status.IdStatus into StatusGroup
                            from Status in StatusGroup.DefaultIfEmpty()
                            join Category in context.Category on Requests.IdCategory equals Category.IdCategory into CategoryGroup
                            from Category in CategoryGroup.DefaultIfEmpty()
                            join Priority in context.Priority on Requests.IdPriority equals Priority.IdPriority into PriorityGroup
                            from Priority in PriorityGroup.DefaultIfEmpty()
                            select new
                            {
                                Requests.IdRequest,
                                Status.NameStatus,
                                Category.NameCategory,
                                Priority.NamePriority,
                                Requests.PlanDate,
                            };

                // Применение фильтрации по приоритетам
                if (priorities.Any())
                {
                    query = query.Where(r => priorities.Contains(r.NamePriority));
                }

                // Применение фильтрации по статусам
                if (statuses.Any())
                {
                    query = query.Where(r => statuses.Contains(r.NameStatus));
                }

                // Применение фильтрации по тексту
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(r => r.NameCategory.Contains(searchText) ||
                                             r.NameStatus.Contains(searchText) ||
                                             r.NamePriority.Contains(searchText));
                }

                // Обновление данных DataGrid
                var filteredData = query.OrderBy(r => r.IdRequest).ToList();
                ModelRequest.Clear();
                foreach (var request in filteredData)
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



        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = ReqestDgList.SelectedItem as ClassRequest;
            if (selectedRequest == null)
            {
                MBClass.ErrorMB("Выбирете заявку");
                return;
            }
            else if (selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Canceled ||
                    selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Overdue ||
                    selectedRequest.IdStatus == (int)RequestHelper.StatusEnum.Completed)
            {
                MBClass.ErrorMB("Заявку уже нельзя отклонить");
            }
            else
            {
                using (var context = new ITAdminEntities())
                {
                    var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                    if (request != null)
                    {
                        request.IdStatus = (int)RequestHelper.StatusEnum.Denied;
                        request.IdExcutor = AuthUser.IdCurretUser;
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void ReqestDgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ReqestDgList.SelectedItem is ClassRequest selectedRequest)
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchedText = SearchTextBox.Text;
            ApplyFilters(new List<string>(), new List<string>(), searchedText);
        }
    }
}

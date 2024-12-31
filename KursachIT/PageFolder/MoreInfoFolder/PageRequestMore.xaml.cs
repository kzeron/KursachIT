using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.MoreFolder
{
    public partial class PageRequestMore : Page
    {
        private readonly int _idRequest;

        public PageRequestMore(int idRequest)
        {
            InitializeComponent();
            _idRequest = idRequest;
            LoadRequestData();
        }

        private void LoadRequestData()
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Получение данных о запросе по ID
                    var requestDetails = context.Requests
                        .Where(r => r.IdRequest == _idRequest)
                        .Select(r => new
                        {
                            StatusName = r.Status.NameStatus,
                            CategoryName = r.Category.NameCategory,
                            PriorityName = r.Priority.NamePriority,
                            r.PlanDate,
                            r.DateRealize,
                            r.Transcription,
                            SenderName = r.Employers.Lastname,
                            ExecutorName = r.Employers.Name
                        })
                        .FirstOrDefault();

                    if (requestDetails != null)
                    {
                        NameStatusLabel.Content = requestDetails.StatusName ?? "Не указано";
                        NameCategoryLabel.Content = requestDetails.CategoryName ?? "Не указано";
                        NamePriorityLabel.Content = requestDetails.PriorityName ?? "Не указано";
                        PlanDateLabel.Content = requestDetails.PlanDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        DateRealizeLabel.Content = requestDetails.DateRealize?.ToString("dd.MM.yyyy") ?? "Не указано";
                        TranscriptionLabel.Content = requestDetails.Transcription ?? "Не указано";
                        NameRequestSenderLabel.Content = requestDetails.SenderName ?? "Не указано";
                        NameExecutorLabel.Content = requestDetails.ExecutorName ?? "Не указано";
                    }
                    else
                    {
                        MBClass.ErrorMB("Информация о запросе не найдена.");
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

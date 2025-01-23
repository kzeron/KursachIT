using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для EditRequest.xaml
    /// </summary>
    public partial class EditRequest : Page
    {
        private Requests _request;
        public int RequestId { get; set; }

        public EditRequest(int requestId)
        {
            InitializeComponent();
            this.RequestId = requestId;
            LoadRequest();
        }

        private void LoadRequest()
        {
            var request = ITAdminEntities.GetContext().Requests
                .FirstOrDefault(r => r.IdRequest == this.RequestId);

            _request = request;
            DataContext = request;

            LoadPriority();
            LoadCategory();

            TranscriptionTb.Text = request.Transcription;
            PriorityCb.SelectedValue = request.IdPriority;
            CategoryCb.SelectedValue = request.IdCategory;
            PlanDatePicker.SelectedDate = request.PlanDate;
        }

        private void LoadPriority()
        {
            var priorityList = ITAdminEntities.GetContext().Priority.ToList();
            PriorityCb.ItemsSource = priorityList;
            PriorityCb.DisplayMemberPath = "NamePriority";
            PriorityCb.SelectedValuePath = "IdPriority";
        }

        private void LoadCategory()
        {
            var categoryList = ITAdminEntities.GetContext().Category.ToList();
            CategoryCb.ItemsSource = categoryList;
            CategoryCb.DisplayMemberPath = "NameCategory";
            CategoryCb.SelectedValuePath = "IdCategory";
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Пожалуйста, выберите приоритет.");
                return;
            }
            if (CategoryCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Пожалуйста, выберите категорию.");
                return;
            }

            try
            {
                if (PriorityCb.SelectedItem is Priority selectedPriority)
                {
                    _request.IdPriority = selectedPriority.IdPriority;
                }

                if (CategoryCb.SelectedItem is Category selectedCategory)
                {
                    _request.IdCategory = selectedCategory.IdCategory;
                }

                _request.Transcription = TranscriptionTb.Text;
                _request.PlanDate = PlanDatePicker.SelectedDate.Value;

                ITAdminEntities.GetContext().SaveChanges();

                MessageBox.Show("Изменения сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Window.GetWindow(this).Close();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex.Message);
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

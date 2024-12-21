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

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для EditRequest.xaml
    /// </summary>
    public partial class EditRequest : Page
    {
        private readonly Requests _currentRequest;
        public EditRequest(Requests currentRequest)
        {
            InitializeComponent();
            currentRequest = _currentRequest;

            PriorityCb.ItemsSource = ITAdminEntities.GetContext().Priority.ToList();
            CategoryCb.ItemsSource = ITAdminEntities.GetContext().Category.ToList();

            LoadRequestData();
            this._currentRequest = currentRequest;
        }

        private void LoadRequestData()
        {
            TranscriptionTb.Text = _currentRequest.Transcription;
            PriorityCb.SelectedValue = _currentRequest.IdPriority;
            CategoryCb.SelectedValue = _currentRequest.IdCategory;
            PlanDatePicker.SelectedDate = _currentRequest.PlanDate;
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите уровень приоритетности");
                return;
            }
            if (CategoryCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите категорию");
                return;
            }

            try
            {
                _currentRequest.IdPriority = (int)PriorityCb.SelectedValue;
                _currentRequest.IdCategory = (int)CategoryCb.SelectedValue;
                _currentRequest.Transcription = TranscriptionTb.Text;
                _currentRequest.PlanDate = (DateTime)PlanDatePicker.SelectedDate;

                ITAdminEntities.GetContext().SaveChanges();
                MBClass.InformationMB("Заявка успешно обновлена.");
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

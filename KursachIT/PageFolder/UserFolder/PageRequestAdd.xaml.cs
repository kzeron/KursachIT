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

namespace KursachIT.PageFolder.UserFolder
{
    /// <summary>
    /// Логика взаимодействия для PageRequestAdd.xaml
    /// </summary>
    public partial class PageRequestAdd : Page
    {
        public PageRequestAdd()
        {
            InitializeComponent();
            PriorityCb.ItemsSource = ITAdminEntities.GetContext().Priority.ToList();
            CategoryCb.ItemsSource = ITAdminEntities.GetContext().Category.ToList();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
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
                // Получение данных из комбобоксов
                var categorySelected = Int32.Parse(CategoryCb.SelectedValue.ToString());
                var prioritySelected = Int32.Parse(PriorityCb.SelectedValue.ToString());
                var planDate = PlanDatePicker.SelectedDate;

                if (planDate == null)
                {
                    MBClass.ErrorMB("Укажите дату выполнения заявки");
                    return;
                }

                // Получение текущего пользователя
                var currentUser = ClassSaveSassion.LoadSession();
                if (currentUser == null)
                {
                    MBClass.ErrorMB("Не удалось получить данные текущей сессии.");
                    return;
                }

                // Получение IdEmployer через IdLogin
                using (var context = new ITAdminEntities())
                {
                    var employee = context.Employers.FirstOrDefault(emp => emp.IdUser == currentUser.IdLogin);
                    if (employee == null)
                    {
                        MBClass.ErrorMB("Не удалось найти данные сотрудника для текущего пользователя.");
                        return;
                    }

                    // Создание новой заявки
                    var newRequest = new Requests
                    {
                        IdStatus = (int)RequestHelper.StatusEnum.New,
                        IdCategory = categorySelected,
                        IdPriority = prioritySelected,
                        Transcription = TranscritionTb.Text,
                        PlanDate = planDate.Value,
                        DateRealize = null,
                        IdRequestSender = employee.IdEmployers, // Используем IdEmployer
                        IdExcutor = null,
                    };

                    // Сохранение заявки в базе данных
                    context.Requests.Add(newRequest);
                    context.SaveChanges();

                    MBClass.InformationMB("Заявка успешно добавлена.");
                    Window.GetWindow(this).Close();
                }
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

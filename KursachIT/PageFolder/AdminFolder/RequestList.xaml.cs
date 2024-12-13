using System;
using System.Collections.Generic;
using System.Linq;
using KursachIT.ClassFolder;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KursachIT.DataFolder;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

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
                MBClass.ErrorMB("Выбирете заявку");
                return;
            }
            else
            {
                using (var context = new ITAdminEntities())
                {
                    var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                    if (request != null)
                    {
                        request.IdStatus = (int)RequestHelper.StatusEnum.InProgress;
                        request.IdExcutor = AuthUser.IdCurretUser;
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void CompliteRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = ReqestDgList.SelectedItem as ClassRequest;
            if (selectedRequest == null)
            {
                MBClass.ErrorMB("Выбирете заявку");
                return;
            }
            else
            {
                using (var context = new ITAdminEntities())
                {
                    var request = context.Requests.FirstOrDefault(r => r.IdRequest == selectedRequest.IdRequst);
                    if (request != null)
                    {

                    }
                }
            }
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

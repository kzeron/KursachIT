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
        private ObservableCollection<ClassRequest> ModelDevices;

        public RequestList()
        {
            
            InitializeComponent();
            ModelDevices = new ObservableCollection<ClassRequest>();
            LoadData();
            ReqestDgList.ItemsSource = ModelDevices;

        }
        public void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                var requestsData = (from Requests in context.Requests 
                                   join Status in context.Status on Requests.IdStatus equals Status.IdStatus
                                   join Category in context.Category on Requests.IdCategory equals Category.IdCategory
                                   join Priority in context.Priority on Requests.IdPriority equals Priority.IdPriority into PriorityGroup
                                   from Priority in PriorityGroup.DefaultIfEmpty()
                                   join Executor in context.Employers on Requests.IdExcutor equals Executor.IdEmployers
                                   join RequestSender in context.Employers on Requests.IdRequestSender equals RequestSender.IdEmployers into RequestSenderGroup
                                   from RequestSender in RequestSenderGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       Requests.Idrequest,
                                       Requests.IdStatus,
                                       Requests.IdCategory,
                                       Requests.IdPriority,
                                       Requests.PlanDate,
                                   }).OrderBy(u => u.Idrequest)
                                   .ToList();
                ModelDevices.Clear();
                foreach (var requests in requestsData)
                {
                    ModelDevices.Add(new ClassRequest
                    {
                       IdRequst = requests.Idrequest,
                       IdStatus = requests.IdStatus,
                       IdCategory = requests.IdCategory,
                       IdPriority = requests.IdPriority,
                       PlanDate = requests.PlanDate,
                    });
                }
                ReqestDgList.ItemsSource = ModelDevices;
                
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new PageStaff());
        }
    }
}

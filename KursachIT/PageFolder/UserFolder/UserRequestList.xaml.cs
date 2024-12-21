using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.PageFolder.AddPages;
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

            // Get the ID of the authorized user (replace with your logic to retrieve user ID)
            int authorizedUserId = GetAuthorizedUserId();

            using (var context = new ITAdminEntities())
            {
                var requestsData = (from request in context.Requests
                                    where request.IdRequestSender == authorizedUserId // Filter by authorized user ID
                                    join status in context.Status on request.IdStatus equals status.IdStatus into statusGroup
                                    from status in statusGroup.DefaultIfEmpty()
                                    join category in context.Category on request.IdCategory equals category.IdCategory into categoryGroup
                                    from category in categoryGroup.DefaultIfEmpty()
                                    join priority in context.Priority on request.IdPriority equals priority.IdPriority into priorityGroup
                                    from priority in priorityGroup.DefaultIfEmpty()
                                    select new
                                    {
                                        request.IdRequest,
                                        status.NameStatus,
                                        category.NameCategory,
                                        priority.NamePriority,
                                        request.PlanDate
                                    })
                                    .OrderBy(u => u.IdRequest)
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
                        PlanDate = request.PlanDate
                    });
                }
                ReqestDgList.ItemsSource = ModelRequest;
            }
        }

        // Replace this with your logic to retrieve the ID of the authorized user
        private int GetAuthorizedUserId()
        {
            // Implement your logic here to get the authorized user ID
            // This could involve checking session data, cookies, or user credentials
            return 1; // Replace with the actual authorized user ID
        }
        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            AnketWin anketWin = new AnketWin(new PageRequestAdd());
            anketWin.Show();

            LoadData();
        }
    }
}

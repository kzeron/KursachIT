using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.PageFolder.AddPages;
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
                using(var context = new ITAdminEntities())
                {
                    var staffData = (from Employers in context.Employers
                                     join User in context.User on Employers.IdUser equals User.IdLogin
                                     join Role in context.Role on User.IdRole equals Role.IdRole into UserGroup
                                     from Role in UserGroup.DefaultIfEmpty()
                                     join Office in context.Office on Employers.IdOffice equals Office.IdOffice
                                     join Cabinet in context.Cabinet on Employers.IdCab equals Cabinet.IdNumberCab into OfficeGroup
                                     from Cabinet in OfficeGroup.DefaultIfEmpty()
                                     select new
                                     {
                                         User.IdLogin,
                                         Role.NameRole,
                                         Employers.Name,
                                         Employers.Lastname,
                                         Employers.Patronymic,
                                         Employers.Office,
                                         Office.NameOffice,
                                         Cabinet.numberCab,
                                         Employers.email,
                                         Employers.numberPhone
                                     }).OrderBy(u => u.IdLogin)
                                     .ToList();
                    _users.Clear();
                    foreach(var user in staffData)
                    {
                        _users.Add(new ClassUser
                        {
                            IdUser = user.IdLogin,
                            Name = user.Name,
                            LastName = user.Lastname,
                            Patronymic = user.Patronymic,
                            NumberOffice = user.numberCab,
                        });
                    }
                    StaffDgList.ItemsSource = _users;
                }
            }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            Windows.AnketWin anketWin = new Windows.AnketWin(new PageAddLogin());
            anketWin.Show(); 
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new DevicesList());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if(StaffDgList.SelectedItem is ClassUser selectedUser)
            {

            }
        }
    }
}

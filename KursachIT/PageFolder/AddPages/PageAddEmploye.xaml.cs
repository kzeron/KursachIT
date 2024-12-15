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

namespace KursachIT.PageFolder.AddPages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEmploye.xaml
    /// </summary>
    public partial class PageAddEmploye : Page
    {
        int _newIdUser;
        public PageAddEmploye(int userId)
        {
            InitializeComponent();
            _newIdUser = userId;
            LoadOffice();
        }
        private void LoadOffice()
        {
            using (var context = new ITAdminEntities())
            {
                var offices = context.Office.ToList();
                var numbers = context.Cabinet.ToList();
                NameOfficeCb.ItemsSource = offices;
                NamberOfficeCb.ItemsSource = numbers;
                
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEmTb.Text))
            {
                MBClass.ErrorMB("Введите имя");
            }
            else if (string.IsNullOrWhiteSpace(SurnameEmTb.Text))
            {
                MBClass.ErrorMB("Введите Фамилию");
            }
            else if (NamberOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберете кабинет");
            }
            else if (NameOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберете кабинет");
            }
            else if (string.IsNullOrWhiteSpace(EmailEmTb.Text))
            {
                MBClass.ErrorMB("Введите почту");
            }
            else if (string.IsNullOrWhiteSpace(PhoneEmTb.Text))
            {
                MBClass.ErrorMB("Укажите телефон");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        var selectedOfficeId = ((Office)NameOfficeCb.SelectedItem).IdOffice;
                        var selectedCabinetId = ((Cabinet)NamberOfficeCb.SelectedItem).IdNumberCab;

                        // Создаем нового сотрудника
                        var employer = new Employers
                        {
                            Name = NameEmTb.Text,
                            Lastname = SurnameEmTb.Text,
                            Patronymic = PathronymicEmTb.Text,
                            numberPhone = PhoneEmTb.Text,
                            email = EmailEmTb.Text,
                            IdOffice = selectedOfficeId, // Передаем ID отдела
                            IdCab = selectedCabinetId, // Передаем ID кабинета
                            IdUser = _newIdUser
                        };
                        context.Employers.Add(employer);
                        context.SaveChanges();
                    }
                    Window.GetWindow(this).Close();
                }
                catch(Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }
        }
        public static class SharedData
        {
            public static int LastUserId { get; set; }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}

using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageEditEmployer.xaml
    /// </summary>
    public partial class PageEditEmployer : Page
    {
        private readonly int _idUser;
        private Employers _currentEmployer;

        public PageEditEmployer(int idUser)
        {
            InitializeComponent();
            _idUser = idUser;
            LoadOfficeAndCabinets();
        }

        private void LoadOfficeAndCabinets()
        {
            using (var context = new ITAdminEntities())
            {
                NameOfficeCb.ItemsSource = context.Office.ToList();
                NameOfficeCb.DisplayMemberPath = "OfficeName";
                NameOfficeCb.SelectedValuePath = "IdOffice";

                NamberOfficeCb.ItemsSource = context.Cabinet.ToList();
                NamberOfficeCb.DisplayMemberPath = "NumberCabinet";
                NamberOfficeCb.SelectedValuePath = "IdNumberCab";
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEmTb.Text))
            {
                MBClass.ErrorMB("Введите имя.");
                return;
            }
            if (string.IsNullOrWhiteSpace(SurnameEmTb.Text))
            {
                MBClass.ErrorMB("Введите фамилию.");
                return;
            }
            if (NamberOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите кабинет.");
                return;
            }
            if (NameOfficeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите офис.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailEmTb.Text))
            {
                MBClass.ErrorMB("Введите почту.");
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneEmTb.Text))
            {
                MBClass.ErrorMB("Введите телефон.");
                return;
            }

            try
            {
                using (var context = new ITAdminEntities())
                {
                    var employer = new Employers
                    {
                        Name = NameEmTb.Text,
                        Lastname = SurnameEmTb.Text,
                        Patronymic = PathronymicEmTb.Text,
                        numberPhone = PhoneEmTb.Text,
                        email = EmailEmTb.Text,
                        IdOffice = (int)NameOfficeCb.SelectedValue,
                        IdCab = (int)NamberOfficeCb.SelectedValue,
                        IdUser = _idUser,
                        IdStatus = 7
                    };

                    context.Employers.Add(employer);
                    context.SaveChanges();

                    MBClass.InformationMB("Сотрудник успешно добавлен.");
                    Window.GetWindow(this)?.Close();
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex.Message);
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }
    }
}


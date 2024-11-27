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
        public PageAddEmploye()
        {
            InitializeComponent();
            LoadOffice();
        }
        private void LoadOffice()
        {
            using (var context = new ITAdminEntities())
            {
                var offices = context.Office.ToList();
                NameOfficeCb.ItemsSource = offices;
                NameOfficeCb.DisplayMemberPath = "NameOffice";
                NameOfficeCb.SelectedValuePath = "IdOffice";
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(NameEmTb.Text))
            {
                if(string.IsNullOrWhiteSpace(SurnameEmTb.Text))
                {
                    if(string.IsNullOrWhiteSpace(PathronymicEmTb.Text))
                    {
                        if(string.IsNullOrWhiteSpace(LoginEmTb.Text))
                        {

                        }
                    }
                }    
            }
        }
    }
}

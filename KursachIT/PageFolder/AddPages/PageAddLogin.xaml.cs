using System;
using KursachIT.Windows;
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
    /// Логика взаимодействия для PageAddLogin.xaml
    /// </summary>
    public partial class PageAddLogin : Page
    {
        public PageAddLogin()
        {
            InitializeComponent();
        }
        private void LoadRole()
        {

        }
        private void AddBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            AnketWin anketWin = new AnketWin();
            anketWin.Close();    
        }
    }
}

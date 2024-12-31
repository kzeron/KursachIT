using KursachIT.ClassFolder;
using KursachIT.PageFolder;
using KursachIT.PageFolder.AdminFolder;
using KursachIT.PageFolder.UserFolder;
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
using System.Windows.Shapes;

namespace KursachIT.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserWindowMain.xaml
    /// </summary>
    public partial class UserWindowMain : Window
    {
        public UserWindowMain()
        {
            InitializeComponent();
            FrameConnect.NavigationService.Navigate(new UserRequestList());
        }
        private void WinMinIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WinCloseIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MBClass.ExitMB();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void LogOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClassSaveSassion.ClearSession();
            new WindowAuth().Show();
            Close();

        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new PersAcc());
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new UserRequestList());
        }
    }
}

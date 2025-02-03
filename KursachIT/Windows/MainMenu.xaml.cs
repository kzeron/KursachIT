using KursachIT.PageFolder.AdminFolder;
using KursachIT.ClassFolder;
using KursachIT.PageFolder;
using System.Windows;
using System.Windows.Input;

namespace KursachIT.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            FrameConnect.NavigationService.Navigate(new RequestList());
        }

        private void Staff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new PageStaff());
        }

        private void DeviceTbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new DevicesList());
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new RequestList());
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

        private void PersCabTb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new PersAcc());
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            FrameConnect.NavigationService.Navigate(new PageHistory());
        }
    }
}

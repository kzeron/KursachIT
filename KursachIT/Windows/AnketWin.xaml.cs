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
    /// Логика взаимодействия для AnketWin.xaml
    /// </summary>
    public partial class AnketWin : Window
    {
        public AnketWin(Page pageToLoad)
        {
            InitializeComponent();
            NavigationAdding.Navigate(pageToLoad);
        }

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void PackIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            bool result = ClassFolder.MBClass.QuestionMB("Закрыть окно?");
            if (result) 
            {
                Close();
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

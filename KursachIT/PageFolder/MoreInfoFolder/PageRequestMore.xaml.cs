using KursachIT.ClassFolder;
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

namespace KursachIT.PageFolder.MoreFolder
{
    /// <summary>
    /// Логика взаимодействия для PageRequestMore.xaml
    /// </summary>
    public partial class PageRequestMore : Page
    {
        public PageRequestMore(ClassRequest selectedRequest)
        {
            InitializeComponent();
            DataContext = selectedRequest;
        }
        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close(); // Закрываем окно
        }
    }
}

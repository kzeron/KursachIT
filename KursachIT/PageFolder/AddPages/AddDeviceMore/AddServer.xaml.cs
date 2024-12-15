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

namespace KursachIT.PageFolder.AddPages.AddDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для AddServer.xaml
    /// </summary>
    public partial class AddServer : Page
    {
        private int _idDevice;
        public AddServer(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

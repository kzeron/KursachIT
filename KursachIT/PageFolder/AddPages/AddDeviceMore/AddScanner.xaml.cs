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

namespace KursachIT.PageFolder.AddPages.AddDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для AddScanner.xaml
    /// </summary>
    public partial class AddScanner : Page
    {
        private int _idDevice;
        public AddScanner(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if(FeederCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите наличие подачика");
            }
            else if(string.IsNullOrWhiteSpace(MaxScanSpeedTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальную скорость сканирования");
            }
            else if(string.IsNullOrWhiteSpace(MaxResolutionTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальное разрешение");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        double maxSpeed;
                        if (!double.TryParse(MaxScanSpeedTb.Text, out maxSpeed))
                        {
                            return;
                        }
                        var selectedFeeder = context.DocumentFeeder.FirstOrDefault(f => f.IdDocumentFeeder == ((DocumentFeeder)FeederCb.SelectedItem).IdDocumentFeeder);
                        var scanner = new ScannerDetails
                        {
                            IdDocumentFeeder = selectedFeeder.IdDocumentFeeder,
                            ScanSpeed = maxSpeed,
                            MaxScanResolution = MaxResolutionTb.Text,
                            IdDevice = _idDevice
                        };
                    }

                }
                catch(Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }

        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KursachIT.PageFolder.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для DevicesList.xaml
    /// </summary>
    public partial class DevicesList : Page
    {
        private ObservableCollection<ClassDevice> ModelDevices;
        public DevicesList()
        {
            InitializeComponent();
            ModelDevices = new ObservableCollection<ClassDevice>();
            LoadData();
            ReqestDgList.ItemsSource = ModelDevices;
        }

            

        public void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                var devicesData = (from Devices in context.Devices
                                   join DeviceTypes in context.DeviceTypes on Devices.IdDeviceType equals DeviceTypes.IdDeviceType
                                   join PCDetails in context.PCDetails on Devices.IdDevice equals PCDetails.IdDevice into pcDetailsGroup
                                   from PCDetails in pcDetailsGroup.DefaultIfEmpty()
                                   join ServerDetails in context.ServerDetails on Devices.IdDevice equals ServerDetails.IdDevice into serverDetailsGroup
                                   from ServerDetails in serverDetailsGroup.DefaultIfEmpty()
                                   join ScannerDetails in context.ScannerDetails on Devices.IdDevice equals ScannerDetails.IdScanner into scannerDetailsGroup
                                   from ScannerDetails in scannerDetailsGroup.DefaultIfEmpty()
                                   join PrinterDetails in context.PrinterDetails on Devices.IdDevice equals PrinterDetails.IdDevice into printerDetailsGroup
                                   from PrinterDetails in printerDetailsGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       Devices.IdDevice,
                                       DeviceTypes.DeviceTypeName,
                                       Devices.SerialNumber,
                                       Devices.PurchaseDate,
                                       Devices.WarrantyEndDate
                                   }).OrderBy(u => u.IdDevice)
                                   .ToList();
                ModelDevices.Clear();
                foreach (var device in devicesData)
                {
                    ModelDevices.Add(new ClassDevice
                    {
                        IdDevice = device.IdDevice,
                        DeviceTypeName = device.DeviceTypeName,
                        SerialNumber = device.SerialNumber,
                        PurchaseDate = device.PurchaseDate,
                        WarrantyEndDate = device.WarrantyEndDate
                    });
                }
                ReqestDgList.ItemsSource = ModelDevices;

            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new PageStaff());
        }

        private void Tasks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RequestList());
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

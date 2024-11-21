using System;
using System.Collections.Generic;
using System.Linq;
using KursachIT.ClassFolder;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KursachIT.DataFolder;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace KursachIT.PageFolder.AdminFolder
{
    /// <summary>
    /// Логика взаимодействия для RequestList.xaml
    /// </summary>
    public partial class RequestList : Page
    {
        private ObservableCollection<ClassDevice>ModelDevices { get; set; }

        public RequestList()
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
                                   join PCDetails in context.PCDetails on DeviceTypes.IdDeviceType equals PCDetails.IdDevice
                                   join ServerDetails in context.ServerDetails on DeviceTypes.IdDeviceType equals ServerDetails.IdDevice
                                   join ScannerDetails in context.ScannerDetails on DeviceTypes.IdDeviceType equals ScannerDetails.IdScanner
                                   join PrinterDetails in context.PrinterDetails on DeviceTypes.IdDeviceType equals PrinterDetails.IdDevice
                                   select new
                                   {
                                       Devices.IdDevice,
                                       DeviceTypes.DeviceTypeName,
                                       PCDetails.Devices
                                   }).OrderBy(u => u.IdDevice)
                                   .ToList();
                ModelDevices.Clear();
                foreach (var device in devicesData)
                {
                    ModelDevices.Add(new ClassDevice
                    {
                        IdDevice = device.IdDevice,
                        DeviceName = device.DeviceTypeName
                    });
                }
                ReqestDgList.ItemsSource = ModelDevices;

            }
        }
    }
}

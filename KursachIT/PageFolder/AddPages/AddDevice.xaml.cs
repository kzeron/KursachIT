using KursachIT.DataFolder;
using KursachIT.ClassFolder;
using KursachIT.PageFolder.AddPages.AddDeviceMore;
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
using KursachIT.PageFolder.AdminFolder;

namespace KursachIT.PageFolder.AddPages
{
    /// <summary>
    /// Логика взаимодействия для AddDevice.xaml
    /// </summary>
    public partial class AddDevice : Page
    {
        public AddDevice()
        {
            InitializeComponent();
            BrandCb.ItemsSource = ITAdminEntities.GetContext().Brand.ToList();
            DeviceTypeCb.ItemsSource = ITAdminEntities.GetContext().DeviceTypes.ToList();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameDevice.Text))
            {

            }
            else if (string.IsNullOrWhiteSpace(PurchaseDatePicker.Text))
            {

            }
            else if (string.IsNullOrWhiteSpace(WarrantyEndDatePicker.Text))
            {

            }
            else if (BrandCb.SelectedItem == null)
            {

            }
            else if (DeviceTypeCb.SelectedItem == null)
            {

            }
            else
            {
                try
                {
                    using(var context = new ITAdminEntities())
                    {
                        var selectedIdTypeDevice = context.DeviceTypes.FirstOrDefault(type => type.IdDeviceType == ((DeviceTypes)DeviceTypeCb.SelectedItem).IdDeviceType);
                        var selectedIdBrand = context.Brand.FirstOrDefault(b => b.IdBrand == ((Brand)BrandCb.SelectedItem).IdBrand);
                        var deivece = new Devices
                        {
                            NameDevice = NameDevice.Text,
                            PurchaseDate = PurchaseDatePicker.SelectedDate.Value,
                            WarrantyEndDate = PurchaseDatePicker.SelectedDate.Value,
                            IdDeviceType = selectedIdTypeDevice.IdDeviceType,
                            IdBrand = selectedIdBrand.IdBrand

                        };
                        context.Devices.Add(deivece);
                        context.SaveChanges();
                        NameDevice.Clear();
                        PurchaseDatePicker.SelectedDate = null;
                        WarrantyEndDatePicker.SelectedDate = null;
                        BrandCb.SelectedItem = null;
                        DeviceTypeCb.SelectedItem = null;
                    }
                }
                catch
                {

                }
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void DeviceTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DeviceTypeCb.SelectedItem is DeviceType selectedDeviceType)
            {
                switch (selectedDeviceType)
                {
                    case DeviceType.PC:
                        NavigationService.Navigate(new AddPC());
                        break;
                    case DeviceType.Server:
                        NavigationService.Navigate(new AddServer());
                        break;
                    case DeviceType.Scanner:
                        NavigationService.Navigate(new AddScanner());
                        break;
                    case DeviceType.Printer:
                        NavigationService.Navigate(new AddPrinter());
                        break;
                }
            }
        }
    }
}

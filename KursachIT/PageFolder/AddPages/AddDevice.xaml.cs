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
        int selectDevice;
        public AddDevice()
        {
            InitializeComponent();
            BrandCb.ItemsSource = ITAdminEntities.GetContext().Brand.ToList();
            DeviceTypeCb.ItemsSource = ITAdminEntities.GetContext().DeviceTypes.ToList();
            EmplCb.ItemsSource = ITAdminEntities.GetContext().Employers.ToList();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameDevice.Text))
            {
                MBClass.ErrorMB("Пожалуйста, введите название устройства.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(PurchaseDatePicker.Text))
            {
                MBClass.ErrorMB("Пожалуйста, выберите дату покупки.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(WarrantyEndDatePicker.Text))
            {
                MBClass.ErrorMB("Пожалуйста, выберите дату окончания гарантии.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(SerialNumberDevice.Text))
            {
                MBClass.ErrorMB("Пожалуйста, введите серийный номер устройства.");
                return;
            }
            else if (BrandCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Пожалуйста, выберите бренд.");
                return;
            }
            else if (DeviceTypeCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Пожалуйста, выберите тип устройства.");
                return;
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        var selectedIdTypeDevice = context.DeviceTypes.FirstOrDefault(type => type.IdDeviceType == ((DeviceTypes)DeviceTypeCb.SelectedItem).IdDeviceType);
                        var selectedIdBrand = context.Brand.FirstOrDefault(b => b.IdBrand == ((Brand)BrandCb.SelectedItem).IdBrand);
                        var selectedIdEmployer = EmplCb.SelectedValue != null? Int32.Parse(EmplCb.SelectedValue.ToString()): (int?)null; 
                        var device = new Devices
                        {
                            NameDevice = NameDevice.Text,
                            PurchaseDate = PurchaseDatePicker.SelectedDate.Value,
                            DateOfReceipt = DateOfReceiptDatePicker.SelectedDate.Value,
                            WarrantyEndDate = WarrantyEndDatePicker.SelectedDate.Value,
                            SerialNumber = SerialNumberDevice.Text,
                            IdDeviceType = selectedIdTypeDevice.IdDeviceType,
                            IdBrand = selectedIdBrand.IdBrand,
                            IdEmployer = selectedIdEmployer
                            // null, если сотрудник не выбран
                        };

                        context.Devices.Add(device);
                        context.SaveChanges();

                        selectDevice = device.IdDevice;

                        // Переход на соответствующую страницу
                        NavigateToPage(selectedIdTypeDevice.IdDeviceType);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MBClass.ErrorMB($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
        }
        private void NavigateToPage(int idDeviceType)
        {
            Page targetPage = DevicePageSelectorcs.GetPageForDeviceType(idDeviceType, selectDevice);

            if (targetPage != null)
            {
                NavigationService?.Navigate(targetPage);
            }
            else
            {
                MBClass.ErrorMB("Неизвестный тип устройства.");
            }
        }
        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

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

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для EditDevice.xaml
    /// </summary>
    public partial class EditDevice : Page
    {
        private int selectedDeviceId;
        private ClassDevice selectedDevice;
        public EditDevice(ClassDevice device)
        {
            InitializeComponent();
            BrandCb.ItemsSource = ITAdminEntities.GetContext().Brand.ToList();
            DeviceTypeCb.ItemsSource = ITAdminEntities.GetContext().DeviceTypes.ToList();
            EmplCb.ItemsSource = ITAdminEntities.GetContext().Employers.ToList();

            selectedDeviceId = device.IdDevice;
            selectedDevice = device;

            NameDevice.Text = selectedDevice.NameDevice;
            PurchaseDatePicker.SelectedDate = selectedDevice.PurchaseDate;
            DateOfReceiptDatePicker.SelectedDate = selectedDevice.DateOfReceipt;
            WarrantyEndDatePicker.SelectedDate = selectedDevice.WarrantyEndDate;
            SerialNumberDevice.Text = selectedDevice.SerialNumber;
            DeviceTypeCb.SelectedItem = DeviceTypeCb.Items.Cast<DeviceTypes>().FirstOrDefault(t => t.IdDeviceType == selectedDevice.IdDeviceType);
            BrandCb.SelectedItem = BrandCb.Items.Cast<Brand>().FirstOrDefault(b => b.IdBrand == selectedDevice.IdBrand);
            EmplCb.SelectedValue = selectedDevice.IdEmployer;
        }


        private void SaveBt_Click(object sender, RoutedEventArgs e)
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
                        var selectedIdEmployer = EmplCb.SelectedValue != null ? Int32.Parse(EmplCb.SelectedValue.ToString()) : (int?)null;

                        // Update existing device data
                        selectedDevice.NameDevice = NameDevice.Text;
                        selectedDevice.PurchaseDate = PurchaseDatePicker.SelectedDate.Value;
                        selectedDevice.DateOfReceipt = DateOfReceiptDatePicker.SelectedDate.Value;
                        selectedDevice.WarrantyEndDate = WarrantyEndDatePicker.SelectedDate.Value;
                        selectedDevice.SerialNumber = SerialNumberDevice.Text;
                        selectedDevice.IdDeviceType = selectedIdTypeDevice.IdDeviceType;
                        selectedDevice.IdBrand = selectedIdBrand.IdBrand;
                        selectedDevice.IdEmployer = selectedIdEmployer;

                        context.SaveChanges();

                        MBClass.InformationMB("Устройство успешно обновлено.");
                        // Consider closing the edit window or reloading data in the parent window
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

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

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
        Devices _devices;
        public int DeviceId { get; set; }
        public EditDevice(int device)
        {
            InitializeComponent();
            DeviceId = device;
            LoadDevice();
        }

        private void LoadDevice()
        {
            var devices = ITAdminEntities.GetContext().Devices
                .FirstOrDefault(i => i.IdDevice == this.DeviceId);

            _devices = devices;
            DataContext = devices;

            LoadBrand();
            LoadDeviceType();
            LoadEmployer();
            BrandCb.SelectedValue = devices.IdBrand;
            DeviceTypeCb.SelectedValue = devices.IdDeviceType;
            EmplCb.SelectedValue = devices.IdEmployer;
            NameDevice.Text = devices.NameDevice;
            PurchaseDatePicker.SelectedDate = devices.PurchaseDate;
            DateOfReceiptDatePicker.SelectedDate = devices.DateOfReceipt;
            WarrantyEndDatePicker.SelectedDate = devices.WarrantyEndDate;
            SerialNumberDevice.Text = devices.SerialNumber;
        }

        private void LoadBrand()
        {
            var brand = ITAdminEntities.GetContext().Brand.ToList(); // Предполагается, что у вас есть таблица Suppliers
            BrandCb.ItemsSource = brand;
            BrandCb.DisplayMemberPath = "NameBrand"; // Убедитесь, что это поле существует
            BrandCb.SelectedValuePath = "IdBrand";
        }
        private void LoadDeviceType()
        {
            var deviceType = ITAdminEntities.GetContext().DeviceTypes.ToList(); // Предполагается, что у вас есть таблица Suppliers
            DeviceTypeCb.ItemsSource = deviceType;
            DeviceTypeCb.DisplayMemberPath = "DeviceTypeName"; // Убедитесь, что это поле существует
            DeviceTypeCb.SelectedValuePath = "IdDeviceType";
        }
        private void LoadEmployer()
        {
            var employer = ITAdminEntities.GetContext().Employers
        .Select(e => new {
            e.Name,
            e.Lastname,
            e.Patronymic,
            e.IdEmployers
        })
        .ToList()
        .Select(e => new {
            FullName = $"{e.Lastname} {e.Name} {e.Patronymic}",
            e.IdEmployers
        });

            EmplCb.ItemsSource = employer.ToList();
            EmplCb.DisplayMemberPath = "FullName";
            EmplCb.SelectedValuePath = "IdEmployers";

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

                    if (BrandCb.SelectedItem is Brand selectedBrand)
                    {
                        _devices.IdBrand = selectedBrand.IdBrand;
                    }

                    if (DeviceTypeCb.SelectedItem is DeviceTypes selectedDeviceType)
                    {
                        _devices.IdDeviceType = selectedDeviceType.IdDeviceType;
                    }

                    if (EmplCb.SelectedItem is Employers selectedEmployers)
                    {
                        _devices.IdEmployer = selectedEmployers.IdEmployers;
                    }

                    _devices.NameDevice = NameDevice.Text;
                    _devices.PurchaseDate = PurchaseDatePicker.SelectedDate.Value;
                    _devices.DateOfReceipt = DateOfReceiptDatePicker.SelectedDate.Value;
                    _devices.WarrantyEndDate = WarrantyEndDatePicker.SelectedDate.Value;
                    _devices.SerialNumber = SerialNumberDevice.Text;
                    ITAdminEntities.GetContext().SaveChanges();

                    MessageBox.Show("Изменения сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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

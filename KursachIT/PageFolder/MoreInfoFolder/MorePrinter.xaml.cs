using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.MoreInfoFolder
{
    /// <summary>
    /// Логика взаимодействия для MorePrinter.xaml
    /// </summary>
    public partial class MorePrinter : Page
    {
        private int _idDevice;

        public MorePrinter(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    var printerDetails = context.PrinterDetails
                        .Where(printer => printer.IdDevice == _idDevice)
                        .Select(printer => new
                        {
                            printer.Devices.NameDevice,
                            printer.Devices.SerialNumber,
                            printer.Devices.PurchaseDate,
                            printer.Devices.WarrantyEndDate,
                            printer.Devices.DateOfReceipt,
                            DeviceType = printer.Devices.DeviceTypes.DeviceTypeName,
                            Brand = printer.Devices.Brand.NameBrand,
                            Employer = printer.Devices.Employers.Lastname,
                            PrintTechnology = printer.PrintTechonology.PrintTechnologyName,
                            printer.MaxResolution,
                            printer.MaxPrintSpeed,
                            ColorTechnology = printer.ColorTechology.ColorTech
                        })
                        .FirstOrDefault();

                    if (printerDetails != null)
                    {
                        // Заполнение элементов интерфейса
                        DeviceNameLabel.Text = printerDetails.NameDevice ?? "Не указано";
                        SerialNumberLabel.Text = printerDetails.SerialNumber ?? "Не указано";
                        PurchaseDateLabel.Text = printerDetails.PurchaseDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        WarrantyEndDateLabel.Text = printerDetails.WarrantyEndDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        DateOfReceiptLabel.Text = printerDetails.DateOfReceipt?.ToString("dd.MM.yyyy") ?? "Не указано";
                        DeviceTypeLabel.Text = printerDetails.DeviceType ?? "Не указано";
                        BrandLabel.Text = printerDetails.Brand ?? "Не указано";
                        EmployerLabel.Text = printerDetails.Employer ?? "Не указано";
                        PrintTechnologyLabel.Text = printerDetails.PrintTechnology ?? "Не указано";
                        MaxResolutionLabel.Text = printerDetails.MaxResolution?.ToString() ?? "Не указано";
                        MaxPrintSpeedLabel.Text = printerDetails.MaxPrintSpeed.ToString() ?? "Не указано";
                        ColorTechLabel.Text = printerDetails.ColorTechnology ?? "Не указано";
                    }
                    else
                    {
                        MBClass.ErrorMB("Информация о принтере не найдена.");
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

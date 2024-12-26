using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursachIT.PageFolder.MoreInfoFolder
{
    /// <summary>
    /// Логика взаимодействия для MoreScanner.xaml
    /// </summary>
    public partial class MoreScanner : Page
    {
        private int _idDevice;

        public MoreScanner(int idDevice)
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
                    var scannerDetails = context.ScannerDetails
                        .Where(scanner => scanner.IdDevice == _idDevice)
                        .Select(scanner => new
                        {
                            scanner.Devices.NameDevice,
                            scanner.Devices.SerialNumber,
                            scanner.Devices.PurchaseDate,
                            scanner.Devices.WarrantyEndDate,
                            scanner.Devices.DateOfReceipt,
                            DeviceType = scanner.Devices.DeviceTypes.DeviceTypeName,
                            Brand = scanner.Devices.Brand.NameBrand,
                            Employer = scanner.Devices.Employers.Lastname,
                            scanner.MaxScanResolution,
                            scanner.ScanSpeed,
                            DocumentFeeder = scanner.DocumentFeeder.DocumentFeederName
                        })
                        .FirstOrDefault();

                    if (scannerDetails != null)
                    {
                        // Заполнение элементов интерфейса
                        DeviceNameLabel.Text = scannerDetails.NameDevice ?? "Не указано";
                        SerialNumberLabel.Text = scannerDetails.SerialNumber ?? "Не указано";
                        PurchaseDateLabel.Text = scannerDetails.PurchaseDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        WarrantyEndDateLabel.Text = scannerDetails.WarrantyEndDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        DateOfReceiptLabel.Text = scannerDetails.DateOfReceipt?.ToString("dd.MM.yyyy") ?? "Не указано";
                        DeviceTypeLabel.Text = scannerDetails.DeviceType ?? "Не указано";
                        BrandLabel.Text = scannerDetails.Brand ?? "Не указано";
                        EmployerLabel.Text = scannerDetails.Employer ?? "Не указано";
                        MaxScanResolutionLabel.Text = scannerDetails.MaxScanResolution?.ToString() ?? "Не указано";
                        ScanSpeedLabel.Text = scannerDetails.ScanSpeed.ToString() ?? "Не указано";
                        DocumentFeederLabel.Text = scannerDetails.DocumentFeeder ?? "Не указано";
                    }
                    else
                    {
                        MBClass.ErrorMB("Информация о сканере не найдена.");
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

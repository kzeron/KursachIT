using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
                            scanner.Devices.PhotoPath,
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

                        if (!string.IsNullOrWhiteSpace(scannerDetails.PhotoPath) && File.Exists(scannerDetails.PhotoPath))
                        {
                            try
                            {
                                DeviceImage.Source = new BitmapImage(new Uri(scannerDetails.PhotoPath));
                            }
                            catch (Exception ex)
                            {
                                MBClass.ErrorMB($"Ошибка загрузки изображения: {ex.Message}");
                            }
                        }
                        else
                        {
                            DeviceImage.Source = null; // Устанавливаем пустое изображение
                            MBClass.InformationMB("Фотография устройства отсутствует или путь неверный.");
                        }
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

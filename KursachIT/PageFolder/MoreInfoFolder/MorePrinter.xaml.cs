using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KursachIT.PageFolder.MoreInfoFolder
{
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
                            printer.Devices.Photo, // Загружаем фото из БД как byte[]
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
                        // Заполнение текстовых полей
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

                        // Загрузка изображения из базы
                        if (printerDetails.Photo != null && printerDetails.Photo.Length > 0)
                        {
                            try
                            {
                                using (var stream = new MemoryStream(printerDetails.Photo))
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.StreamSource = stream;
                                    bitmap.EndInit();
                                    bitmap.Freeze(); // Делаем потокобезопасным
                                    PhotoImage.Source = bitmap; // Устанавливаем изображение
                                }
                            }
                            catch (Exception ex)
                            {
                                MBClass.ErrorMB($"Ошибка загрузки изображения: {ex.Message}");
                            }
                        }
                        else
                        {
                            PhotoImage.Source = null; // Очищаем, если фото нет
                            MBClass.InformationMB("Фотография устройства отсутствует.");
                        }
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

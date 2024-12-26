using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using KursachIT.PageFolder.AddPages;
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

namespace KursachIT.PageFolder.MoreInfoFolder
{
    /// <summary>
    /// Логика взаимодействия для MoreServer.xaml
    /// </summary>
    public partial class MoreServer : Page
    {
        private int _idDevice;
        public MoreServer(int idDevice)
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
                    var serverDetails = context.ServerDetails
                        .Where(server => server.IdDevice == _idDevice)
                        .Select(server => new
                        {
                            server.Devices.NameDevice,
                            server.Devices.SerialNumber,
                            server.Devices.PurchaseDate,
                            server.Devices.WarrantyEndDate,
                            server.Devices.DateOfReceipt,
                            DeviceType = server.Devices.DeviceTypes.DeviceTypeName,
                            Brand = server.Devices.Brand.NameBrand,
                            Employer = server.Devices.Employers.Lastname,
                            server.CPU,
                            server.RAM,
                            server.Storage,
                            server.RackUnit,
                            server.NetworkInterface
                        })
                        .FirstOrDefault();

                    if (serverDetails != null)
                    {
                        // Заполнение элементов интерфейса
                        DeviceNameLabel.Text = serverDetails.NameDevice ?? "Не указано";
                        SerialNumberLabel.Text = serverDetails.SerialNumber ?? "Не указано";
                        PurchaseDateLabel.Text = serverDetails.PurchaseDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        WarrantyEndDateLabel.Text = serverDetails.WarrantyEndDate.ToString("dd.MM.yyyy") ?? "Не указано";
                        DateOfReceiptLabel.Text = serverDetails.DateOfReceipt?.ToString("dd.MM.yyyy") ?? "Не указано";
                        DeviceTypeLabel.Text = serverDetails.DeviceType ?? "Не указано";
                        BrandLabel.Text = serverDetails.Brand ?? "Не указано";
                        EmployerLabel.Text = serverDetails.Employer ?? "Не указано";
                        CPULabel.Text = serverDetails.CPU ?? "Не указано";
                        RAMLabel.Text = serverDetails.RAM.ToString() ?? "Не указано";
                        StorageLabel.Text = serverDetails.Storage ?? "Не указано";
                        RackUnitLabel.Text = serverDetails.RackUnit.ToString() ?? "Не указано";
                        NetworkInterfaceLabel.Text = serverDetails.NetworkInterface ?? "Не указано";
                    }
                    else
                    {
                        MBClass.ErrorMB("Информация о сервере не найдена.");
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

using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System.Data.Entity;
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
    /// Логика взаимодействия для MorePC.xaml
    /// </summary>
    public partial class MorePC : Page
    {
        private int _idDevice;
        public MorePC(int idDevice)
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
                    var pcDetails = context.PCDetails
                        .Include(pc => pc.Devices.DeviceTypes)
                        .Include(pc => pc.Devices.Brand)
                        .Include(pc => pc.Devices.Employers)
                        .Where(pc => pc.IdDevice == _idDevice)
                        .Select(pc => new
                        {
                            pc.Devices.NameDevice,
                            pc.Devices.SerialNumber,
                            pc.Devices.PurchaseDate,
                            pc.Devices.WarrantyEndDate,
                            pc.Devices.DateOfReceipt,
                            DeviceTypeName = pc.Devices.DeviceTypes.DeviceTypeName,
                            BrandName = pc.Devices.Brand.NameBrand,
                            EmployerLastname = pc.Devices.Employers.Lastname,
                            pc.CPU,
                            pc.RAM,
                            pc.Storage,
                            pc.GPU,
                            pc.Devices.Photo // Теперь загружаем `Photo`, а не `PhotoPath`
                        })
                        .FirstOrDefault();

                    if (pcDetails != null)
                    {
                        DataContext = pcDetails;

                        // Загрузка изображения из `byte[]`
                        if (pcDetails.Photo != null && pcDetails.Photo.Length > 0)
                        {
                            using (var stream = new System.IO.MemoryStream(pcDetails.Photo))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = stream;
                                bitmap.EndInit();
                                PhotoImage.Source = bitmap;
                            }
                        }
                        else
                        {
                            PhotoImage.Source = null; // Или установить заглушку
                        }
                    }
                    else
                    {
                        MBClass.ErrorMB("Информация не найдена.");
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка: {ex.Message}");
            }
        }



        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

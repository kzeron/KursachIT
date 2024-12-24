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

namespace KursachIT.PageFolder.AddPages.AddDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для AddServer.xaml
    /// </summary>
    public partial class AddServer : Page
    {
        private int _idDevice;
        public AddServer(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Проверяем, существует ли устройство
                    var device = context.Devices.FirstOrDefault(d => d.IdDevice == _idDevice);

                    if (device == null)
                    {
                        MessageBox.Show("Устройство не найдено. Проверьте идентификатор устройства.",
                                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    int ramSize;
                    if (!int.TryParse(RAMTb.Text, out ramSize))
                    {
                        return;
                    }
                    int rackUnit;
                    if(!int.TryParse(RackUnitTb.Text, out rackUnit))
                    {
                        return;
                    }
                    // Создаем запись о сервере
                    var serverDetails = new ServerDetails
                    {
                        IdDevice = _idDevice,
                        CPU = CPUTb.Text.Trim(),
                        RAM = ramSize,
                        Storage = StorageTb.Text.Trim(),
                        RackUnit = rackUnit,
                        NetworkInterface = NetworkInterfaceTb.Text.Trim(),
                    };

                    // Добавляем запись в таблицу ServerDetails
                    context.ServerDetails.Add(serverDetails);
                    context.SaveChanges();

                    MessageBox.Show("Сервер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Переход назад или обновление страницы
                    Window.GetWindow(this).Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сервера: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    // Находим устройство по его IdDevice
                    var deviceToRemove = context.Devices.FirstOrDefault(d => d.IdDevice == _idDevice);

                    if (deviceToRemove != null)
                    {
                        // Удаляем устройство
                        context.Devices.Remove(deviceToRemove);
                        context.SaveChanges();

                        MBClass.InformationMB("Устройство успешно удалено.");
                    }
                    else
                    {
                        MBClass.ErrorMB("Устройство с указанным Id не найдено.");
                    }
                }

                // Возвращение на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка при удалении устройства: {ex.Message}");
            }
        }

    }
}

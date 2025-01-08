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

namespace KursachIT.PageFolder.EditPages.EditDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для EditServer.xaml
    /// </summary>
    public partial class EditServer : Page
    {
        private int _idDevice;
        private ServerDetails _currentServer;
        public EditServer(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                _currentServer = context.ServerDetails.FirstOrDefault(s => s.IdDevice == _idDevice);

                if (_currentServer != null)
                {
                    CPUTb.Text = _currentServer.CPU;
                    RAMTb.Text = _currentServer.RAM.ToString();
                    StorageTb.Text = _currentServer.Storage;
                    RackUnitTb.Text = _currentServer.RackUnit.ToString();
                    NetworkInterfaceTb.Text = _currentServer.NetworkInterface;
                }
                else
                {
                    MBClass.ErrorMB("Данные о сервере не найдены.");
                }
            }
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CPUTb.Text))
            {
                MBClass.ErrorMB("Введите CPU.");
                return;
            }
            if (string.IsNullOrWhiteSpace(RAMTb.Text) || !int.TryParse(RAMTb.Text, out int ramSize))
            {
                MBClass.ErrorMB("Введите корректное значение RAM.");
                return;
            }
            if (string.IsNullOrWhiteSpace(StorageTb.Text))
            {
                MBClass.ErrorMB("Введите объем хранилища.");
                return;
            }
            if (string.IsNullOrWhiteSpace(RackUnitTb.Text) || !int.TryParse(RackUnitTb.Text, out int rackUnit))
            {
                MBClass.ErrorMB("Введите корректное значение Rack Unit.");
                return;
            }
            if (string.IsNullOrWhiteSpace(NetworkInterfaceTb.Text))
            {
                MBClass.ErrorMB("Введите сетевой интерфейс.");
                return;
            }

            try
            {
                using (var context = new ITAdminEntities())
                {
                    if (_currentServer == null)
                    {
                        MBClass.ErrorMB("Данные о сервере не найдены.");
                        return;
                    }

                    // Обновляем данные
                    _currentServer.CPU = CPUTb.Text.Trim();
                    _currentServer.RAM = ramSize;
                    _currentServer.Storage = StorageTb.Text.Trim();
                    _currentServer.RackUnit = rackUnit;
                    _currentServer.NetworkInterface = NetworkInterfaceTb.Text.Trim();

                    context.Entry(_currentServer).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    MBClass.InformationMB("Изменения успешно сохранены.");
                    Window.GetWindow(this).Close();
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка при удалении устройства: {ex.Message}");
            }
        }
    }
}

using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Linq;
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
    /// Логика взаимодействия для AddPC.xaml
    /// </summary>
    public partial class AddPC : Page
    {
        private int _idDevice;
        public AddPC(int idDivece)
        {
            _idDevice = idDivece;
            InitializeComponent();
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CPUTb.Text))
            {
                MBClass.ErrorMB("Введите процессор");
            }
            else if (string.IsNullOrWhiteSpace(RAMTb.Text))
            {
                MBClass.ErrorMB("Введите объем ОЗУ");
            }
            else if (string.IsNullOrWhiteSpace(StorageTb.Text))
            {
                MBClass.ErrorMB("Введите объем памяти");
            }
            else if (string.IsNullOrWhiteSpace(GPUTb.Text))
            {
                MBClass.ErrorMB("Введите модель видеокарты");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        int ramSize;
                        if(!int.TryParse(RAMTb.Text, out ramSize))
                        {
                            return;
                        }
                        var pc = new PCDetails
                        {
                            CPU = CPUTb.Text,
                            RAM = ramSize,
                            Storage = StorageTb.Text,
                            GPU = GPUTb.Text,
                            IdDevice = _idDevice
                        };
                        context.PCDetails.Add(pc);
                        context.SaveChanges();
                    }
                    Window.GetWindow(this).Close();
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
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
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка при удалении устройства: {ex.Message}");
            }
        }

    }
}

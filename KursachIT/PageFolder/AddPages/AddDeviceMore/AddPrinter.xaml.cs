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
    /// Логика взаимодействия для AddPrinter.xaml
    /// </summary>
    public partial class AddPrinter : Page
    {
        private int _idDevice;
        public AddPrinter(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            PrintCb.ItemsSource = ITAdminEntities.GetContext().PrintTechonology.ToList();
            ColorCb.ItemsSource =ITAdminEntities.GetContext().ColorTechology.ToList();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (PrintCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите технологию печати");
            }
            else if (ColorCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите поддерживаемые цвета");
            }
            else if (string.IsNullOrWhiteSpace(MaxResolutionTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальное разрешение");
            }
            else if (string.IsNullOrWhiteSpace(MaxResolutionTb.Text) && !ClassDataValidator.IsResolutionValid(MaxResolutionTb.Text))
            {
                MBClass.ErrorMB("Разрешение должно быть указано в формате 'ширина*высота'.");
            }
            else if (string.IsNullOrWhiteSpace(MaxPrintSpeedTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальную скорость печати");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        // Проверяем, что скорость печати — число
                        if (!double.TryParse(MaxPrintSpeedTb.Text, out double maxSpeed) || maxSpeed <= 0)
                        {
                            MBClass.ErrorMB("Максимальная скорость печати должна быть положительным числом.");
                            return;
                        }

                        // Получаем выбранные технологии
                        var selectedPrintTech = context.PrintTechonology
                            .FirstOrDefault(p => p.IdPrintTechnology == ((PrintTechonology)PrintCb.SelectedItem).IdPrintTechnology);
                        var selectedColorTech = context.ColorTechology
                            .FirstOrDefault(c => c.IdColorTech == ((ColorTechology)ColorCb.SelectedItem).IdColorTech);

                        // Создаём новый объект PrinterDetails
                        var printer = new PrinterDetails
                        {
                            IdPrintTechnology = selectedPrintTech.IdPrintTechnology,
                            IdColorTech = selectedColorTech.IdColorTech,
                            MaxResolution = MaxResolutionTb.Text,
                            MaxPrintSpeed = maxSpeed,
                            IdDevice = _idDevice
                        };

                        // Добавляем и сохраняем
                        context.PrinterDetails.Add(printer);
                        context.SaveChanges();
                    }

                    Window.GetWindow(this).Close();
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB($"Ошибка: {ex.Message}");
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

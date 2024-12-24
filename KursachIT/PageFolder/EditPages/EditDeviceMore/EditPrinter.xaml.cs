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
    /// Логика взаимодействия для EditPrinter.xaml
    /// </summary>
    public partial class EditPrinter : Page
    {
        private int _idDevice;
        private PrinterDetails _currentPrinter;
        public EditPrinter(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            LoadData();
        }
        private void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                _currentPrinter = context.PrinterDetails.FirstOrDefault(p => p.IdDevice == _idDevice);

                if (_currentPrinter != null)
                {
                    PrintCb.ItemsSource = context.PrintTechonogy.ToList();
                    ColorCb.ItemsSource = context.ColorTechology.ToList();

                    // Устанавливаем выбранные значения
                    PrintCb.SelectedItem = context.PrintTechonogy.FirstOrDefault(pt => pt.IdPrintTechonogy == _currentPrinter.IdPrintTechnology);
                    ColorCb.SelectedItem = context.ColorTechology.FirstOrDefault(ct => ct.IdColorTech == _currentPrinter.IdColorTech);

                    MaxResolutionTb.Text = _currentPrinter.MaxResolution;
                    MaxPrintSpeedTb.Text = _currentPrinter.MaxPrintSpeed.ToString();
                }
            }
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            if (PrintCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите технологию печати");
                return;
            }
            else if (ColorCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите поддерживаемые цвета");
                return;
            }
            else if (string.IsNullOrWhiteSpace(MaxResolutionTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальное разрешение");
                return;
            }
            else if (string.IsNullOrWhiteSpace(MaxPrintSpeedTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальную скорость печати");
                return;
            }

            try
            {
                using (var context = new ITAdminEntities())
                {
                    double maxSpeed;
                    if (!double.TryParse(MaxPrintSpeedTb.Text, out maxSpeed))
                    {
                        MBClass.ErrorMB("Ошибка ввода максимальной скорости печати");
                        return;
                    }

                    if (_currentPrinter == null)
                    {
                        MBClass.ErrorMB("Принтер не найден в базе данных.");
                        return;
                    }

                    var selectedPrintTech = (PrintTechonogy)PrintCb.SelectedItem;
                    var selectedColorTech = (ColorTechology)ColorCb.SelectedItem;

                    // Обновляем данные принтера
                    _currentPrinter.IdPrintTechnology = selectedPrintTech.IdPrintTechonogy;
                    _currentPrinter.IdColorTech = selectedColorTech.IdColorTech;
                    _currentPrinter.MaxResolution = MaxResolutionTb.Text;
                    _currentPrinter.MaxPrintSpeed = maxSpeed;

                    context.Entry(_currentPrinter).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    MessageBox.Show("Изменения успешно сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window.GetWindow(this).Close();
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка: {ex.Message}");
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

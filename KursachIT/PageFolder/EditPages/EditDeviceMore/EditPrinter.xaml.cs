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
        public int PrinterId { get; set; }
        private PrinterDetails _currentPrinter;
        public EditPrinter(int idDevice)
        {
            InitializeComponent();
            this.PrinterId = idDevice;
            LoadData();
        }
        private void LoadPrint()
        {
            var printer = ITAdminEntities.GetContext().PrintTechonology.ToList();
            PrintCb.ItemsSource = printer;
            PrintCb.DisplayMemberPath = "PrintTechnologyName";
            PrintCb.SelectedValuePath = "IdPrintTechnology";
        }
        private void LoadColor()
        {
            var color = ITAdminEntities.GetContext().ColorTechology.ToList();
            ColorCb.ItemsSource = color;
            ColorCb.DisplayMemberPath = "ColorTech";
            ColorCb.SelectedValuePath = "IdColorTech";
        }
        private void LoadData()
        {

            var printerDetails = ITAdminEntities.GetContext().PrinterDetails.FirstOrDefault(p => p.IdDevice == PrinterId);

            _currentPrinter = printerDetails;
            DataContext = printerDetails;

            LoadPrint(); // Устанавливаем ItemsSource для PrintCb
            LoadColor(); // Устанавливаем ItemsSource для ColorCb

            // Устанавливаем выбранные значения только после загрузки данных
            PrintCb.SelectedValue = _currentPrinter.IdPrintTechnology;
            ColorCb.SelectedValue = _currentPrinter.IdColorTech;
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

                if (PrintCb.SelectedItem is PrintTechonology selectedSupplier)
                {
                    _currentPrinter.IdPrintTechnology = selectedSupplier.IdPrintTechnology; // Обновление IdSupplier
                }

                if (ColorCb.SelectedItem is ColorTechology selectedColor)
                {
                    _currentPrinter.IdColorTech = selectedColor.IdColorTech; // Обновление IdSupplier
                }

                _currentPrinter.MaxResolution = MaxResolutionTb.Text;
                _currentPrinter.MaxPrintSpeed = double.Parse(MaxPrintSpeedTb.Text);

                ITAdminEntities.GetContext().Entry(_currentPrinter).State = System.Data.Entity.EntityState.Modified;
                ITAdminEntities.GetContext().SaveChanges();

                MessageBox.Show("Изменения успешно сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Window.GetWindow(this).Close();

            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка: {ex.Message}");
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
             NavigationService.GoBack();
        }
    }
}

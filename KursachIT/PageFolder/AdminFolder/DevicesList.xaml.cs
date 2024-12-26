    using KursachIT.ClassFolder;
    using KursachIT.DataFolder;
    using KursachIT.PageFolder.AddPages;
    using KursachIT.PageFolder.EditPages;
    using KursachIT.Windows;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
    using static MaterialDesignThemes.Wpf.Theme;

    namespace KursachIT.PageFolder.AdminFolder
    {
        /// <summary>
        /// Логика взаимодействия для DevicesList.xaml
        /// </summary>
        public partial class DevicesList : Page
        {
            private ObservableCollection<ClassDevice> ModelDevices;
            public DevicesList()
            {
                InitializeComponent();
                ModelDevices = new ObservableCollection<ClassDevice>();
                LoadData();
                DevicesDgList.ItemsSource = ModelDevices;
            }



            public void LoadData()
            {
                using (var context = new ITAdminEntities())
                {
                    var devicesData = (from Devices in context.Devices
                                       join DeviceTypes in context.DeviceTypes on Devices.IdDeviceType equals DeviceTypes.IdDeviceType
                                       join PCDetails in context.PCDetails on Devices.IdDevice equals PCDetails.IdDevice into pcDetailsGroup
                                       from PCDetails in pcDetailsGroup.DefaultIfEmpty()
                                       join ServerDetails in context.ServerDetails on Devices.IdDevice equals ServerDetails.IdDevice into serverDetailsGroup
                                       from ServerDetails in serverDetailsGroup.DefaultIfEmpty()
                                       join ScannerDetails in context.ScannerDetails on Devices.IdDevice equals ScannerDetails.IdScanner into scannerDetailsGroup
                                       from ScannerDetails in scannerDetailsGroup.DefaultIfEmpty()
                                       join PrinterDetails in context.PrinterDetails on Devices.IdDevice equals PrinterDetails.IdDevice into printerDetailsGroup
                                       from PrinterDetails in printerDetailsGroup.DefaultIfEmpty()
                                       join Brand in context.Brand on Devices.IdBrand equals Brand.IdBrand into brandDetailsGroup
                                       from Brand in brandDetailsGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           Devices.IdDevice,
                                           Devices.NameDevice,
                                           DeviceTypes.DeviceTypeName,
                                           Devices.SerialNumber,
                                           Devices.PurchaseDate,
                                           Devices.WarrantyEndDate,
                                           Devices.DateOfReceipt,
                                           Brand.NameBrand
                                       }).OrderBy(u => u.IdDevice)
                                       .ToList();
                    ModelDevices.Clear();
                    foreach (var device in devicesData)
                    {
                        ModelDevices.Add(new ClassDevice
                        {
                            IdDevice = device.IdDevice,
                            NameDevice = device.NameDevice,
                            DeviceTypeName = device.DeviceTypeName,
                            SerialNumber = device.SerialNumber,
                            PurchaseDate = device.PurchaseDate,
                            WarrantyEndDate = device.WarrantyEndDate,
                            BrandName = device.NameBrand
                        });
                    }
                    DevicesDgList.ItemsSource = ModelDevices;

                }
            }

            private void AddBt_Click(object sender, RoutedEventArgs e)
            {
                AnketWin anketWin = new AnketWin(new AddDevice());
                anketWin.Show();

                anketWin.Closed += OnAnketWinClosed;
            }
            private void MenuItem_Click(object sender, RoutedEventArgs e)
            {
                if (DevicesDgList.SelectedItem is ClassDevice selectedDevice)
                {
                    var editDeviceWindow = new AnketWin(new EditDevice(selectedDevice.IdDevice));
                    editDeviceWindow.Show();
                    editDeviceWindow.Closed += OnAnketWinClosed;
                }
                else
                {
                    MBClass.ErrorMB("Выберите устройство для редактирования.");
                }
            }


            private void DeleteClick(object sender, RoutedEventArgs e)
            {
                if (DevicesDgList.SelectedItem is ClassDevice selectedDevice)
                {
                    bool result = MBClass.QuestionMB($"Вы уверены, что хотите удалить устройство {selectedDevice.NameDevice} с серийным номером {selectedDevice.SerialNumber}?");

                    if (result)
                    {
                        try
                        {
                            using (var context = new ITAdminEntities())
                            {
                                // Find the device to delete based on its ID
                                var deviceToDelete = context.Devices.FirstOrDefault(d => d.IdDevice == selectedDevice.IdDevice);

                                if (deviceToDelete != null)
                                {
                                    // Remove related details (if applicable)
                                    context.PCDetails.RemoveRange(context.PCDetails.Where(p => p.IdDevice == selectedDevice.IdDevice));
                                    context.ServerDetails.RemoveRange(context.ServerDetails.Where(s => s.IdDevice == selectedDevice.IdDevice));
                                    context.ScannerDetails.RemoveRange(context.ScannerDetails.Where(sc => sc.IdDevice == selectedDevice.IdDevice));
                                    context.PrinterDetails.RemoveRange(context.PrinterDetails.Where(p => p.IdDevice == selectedDevice.IdDevice));

                                    // Remove the device itself
                                    context.Devices.Remove(deviceToDelete);

                                    context.SaveChanges();
                                    MBClass.InformationMB("Устройство успешно удалено.");
                                    LoadData();
                                }
                                else
                                {
                                    MBClass.ErrorMB("Устройство не найдено.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MBClass.ErrorMB($"Произошла ошибка при удалении: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MBClass.ErrorMB("Выберите устройство для удаления.");
                }
            }
            private void OpenFilterPopup_Click(object sender, RoutedEventArgs e)
            {
                // Открытие/закрытие Popup
                FilterPopup.IsOpen = !FilterPopup.IsOpen;
            }

            private void ApplyFilters_Click(object sender, RoutedEventArgs e)
            {
                // Сбор данных фильтров
                var selectedDeviceTypes = new List<string>();
                if (PCCheckBox.IsChecked == true) selectedDeviceTypes.Add("ПК");
                if (ServerCheckBox.IsChecked == true) selectedDeviceTypes.Add("Сервер");
                if (PrinterCheckBox.IsChecked == true) selectedDeviceTypes.Add("Принтер");
                if (ScannerCheckBox.IsChecked == true) selectedDeviceTypes.Add("Сканер");

                var selectedBrands = new List<string>();
                if (GinzzuCheckBox.IsChecked == true) selectedBrands.Add("Ginzzu");
                if (HPCheckBox.IsChecked == true) selectedBrands.Add("HP");
                if (AURORACheckBox.IsChecked == true) selectedBrands.Add("AURORA");
                if (TREIDCheckBox.IsChecked == true) selectedBrands.Add("TREIDCOMPUTERS");
                if (PanasonicCheckBox.IsChecked == true) selectedBrands.Add("Panasonic");
                if (DellCheckBox.IsChecked == true) selectedBrands.Add("Dell");
                if (IBMCheckBox.IsChecked == true) selectedBrands.Add("IBM");
                if (CanonCheckBox.IsChecked == true) selectedBrands.Add("Canon");
                if (XeroxCheckBox.IsChecked == true) selectedBrands.Add("Xerox");

                var searchText = SearchTextBox.Text;

                // Применение фильтра
                ApplyFilters(selectedDeviceTypes, selectedBrands, searchText);

                // Закрытие Popup
                FilterPopup.IsOpen = false;
            }

            public void ApplyFilters(List<string> selectedTypes, List<string> selectedBrands, string searchText)
            {
                using (var context = new ITAdminEntities())
                {
                    // Основной запрос с фильтрами
                    var filteredData = (from Devices in context.Devices
                                        join DeviceTypes in context.DeviceTypes on Devices.IdDeviceType equals DeviceTypes.IdDeviceType
                                        join Brand in context.Brand on Devices.IdBrand equals Brand.IdBrand into brandDetailsGroup
                                        from Brand in brandDetailsGroup.DefaultIfEmpty()
                                        where (selectedTypes.Count == 0 || selectedTypes.Contains(DeviceTypes.DeviceTypeName)) &&
                                              (selectedBrands.Count == 0 || selectedBrands.Contains(Brand.NameBrand)) &&
                                              (searchText == null || searchText.Trim() == "" || Devices.NameDevice.Contains(searchText))
                                        select new
                                        {
                                            Devices.IdDevice,
                                            Devices.NameDevice,
                                            DeviceTypes.DeviceTypeName,
                                            Devices.SerialNumber,
                                            Devices.PurchaseDate,
                                            Devices.WarrantyEndDate,
                                            Devices.DateOfReceipt,
                                            Brand.NameBrand
                                        }).OrderBy(u => u.IdDevice)
                                        .ToList();

                    // Очистка и заполнение модели устройств
                    ModelDevices.Clear();
                    foreach (var device in filteredData)
                    {
                        ModelDevices.Add(new ClassDevice
                        {
                            IdDevice = device.IdDevice,
                            NameDevice = device.NameDevice,
                            DeviceTypeName = device.DeviceTypeName,
                            SerialNumber = device.SerialNumber,
                            PurchaseDate = device.PurchaseDate,
                            WarrantyEndDate = device.WarrantyEndDate,
                            BrandName = device.NameBrand
                        });
                    }

                    // Обновляем источник данных для DataGrid
                    DevicesDgList.ItemsSource = ModelDevices;
                }
            }


            private void DevicesDgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                if (DevicesDgList.SelectedItem is ClassDevice selectedDevice)
                {
                    // Получение типа устройства
                    using (var context = new ITAdminEntities())
                    {
                        var deviceType = context.Devices
                                                .Where(d => d.IdDevice == selectedDevice.IdDevice)
                                                .Select(d => d.IdDeviceType)
                                                .FirstOrDefault();

                        // Проверяем, что тип устройства найден
                        if (deviceType != 0)
                        {
                            // Получение страницы для выбранного типа устройства
                            var page = DevicePageSelectorcs.GetMorePageForDeviceType(deviceType, selectedDevice.IdDevice);

                            if (page != null)
                            {
                                // Открываем окно с нужной страницей
                                AnketWin window = new AnketWin(page);
                                window.Show();
                            }
                            else
                            {
                                MBClass.ErrorMB("Не удалось определить страницу для данного типа устройства.");
                            }
                        }
                        else
                        {
                            MBClass.ErrorMB("Тип устройства не найден.");
                        }
                    }
                }
                else
                {
                    MBClass.ErrorMB("Выберите устройство из списка.");
                }
            }
            private void OnAnketWinClosed(object sender, EventArgs e)
            {
                LoadData();
            }
            private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                var searchText = SearchTextBox.Text;
                ApplyFilters(new List<string>(), new List<string>(), searchText);
            }
        }
    }

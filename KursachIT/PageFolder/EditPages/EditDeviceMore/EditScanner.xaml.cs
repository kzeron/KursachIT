﻿using KursachIT.ClassFolder;
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
    /// Логика взаимодействия для EditScanner.xaml
    /// </summary>
    public partial class EditScanner : Page
    {
        private int _idDevice;
        private ScannerDetails _currentScanner;
        public EditScanner(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            LoadData();
        }
        private void LoadDocumentFeeder()
        {
            var feeder = ITAdminEntities.GetContext().DocumentFeeder.ToList();
            FeederCb.ItemsSource = feeder;
            FeederCb.DisplayMemberPath = "DocumentFeederName";
            FeederCb.SelectedValuePath = "IdDocumentFeeder";
        }
        private void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                _currentScanner = context.ScannerDetails.FirstOrDefault(s => s.IdDevice == _idDevice);

                if (_currentScanner != null)
                {
                    
                    LoadDocumentFeeder();
                    FeederCb.SelectedValue = _currentScanner.IdDocumentFeeder;
                    MaxScanSpeedTb.Text = _currentScanner.ScanSpeed.ToString();
                    MaxResolutionTb.Text = _currentScanner.MaxScanResolution;
                }
            }
        }
        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Возвращение на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB($"Ошибка при удалении устройства: {ex.Message}");
            }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            if (FeederCb.SelectedItem == null)
            {
                MBClass.ErrorMB("Укажите наличие податчика");
                return;
            }
            else if (string.IsNullOrWhiteSpace(MaxScanSpeedTb.Text))
            {
                MBClass.ErrorMB("Укажите максимальную скорость сканирования");
                return;
            }
            else if (string.IsNullOrWhiteSpace(MaxResolutionTb.Text) && !ClassDataValidator.IsResolutionValid(MaxResolutionTb.Text))
            {
                MBClass.ErrorMB("Разрешение должно быть указано в формате 'ширина*высота' (например, 1920*1080).");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        double maxSpeed;
                        if (!double.TryParse(MaxScanSpeedTb.Text, out maxSpeed))
                        {
                            MBClass.ErrorMB("Ошибка ввода скорости сканирования");
                            return;
                        }

                        if (_currentScanner == null)
                        {
                            MBClass.ErrorMB("Сканер не найден в базе данных.");
                            return;
                        }

                        var selectedFeeder = (DocumentFeeder)FeederCb.SelectedItem;

                        // Обновляем данные сканера
                        _currentScanner.IdDocumentFeeder = selectedFeeder.IdDocumentFeeder;
                        _currentScanner.ScanSpeed = maxSpeed;
                        _currentScanner.MaxScanResolution = MaxResolutionTb.Text;

                        context.Entry(_currentScanner).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        MBClass.InformationMB("Изменения успешно сохранены.");
                        Window.GetWindow(this).Close();
                    }
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB($"Ошибка: {ex.Message}");
                }
            }
            
        }
    }
}

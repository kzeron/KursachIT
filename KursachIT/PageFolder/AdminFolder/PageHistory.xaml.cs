using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KursachIT.PageFolder.AdminFolder
{
    public partial class PageHistory : Page
    {
        private ObservableCollection<ClassHistory> _historyData;

        public PageHistory()
        {
            InitializeComponent();
            _historyData = new ObservableCollection<ClassHistory>();
            LoadData();
            HistoryDgList.ItemsSource = _historyData;
        }

        public void LoadData()
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    var historyData = (from history in context.OperationHistory
                                       join employer in context.Employers on history.EmployerId equals employer.IdEmployers
                                       select new ClassHistory
                                       {
                                           IdHistory = history.IdHistory,
                                           TableName = history.TableName,
                                           OperationType = history.OperationType,
                                           OperationTime = history.OperationTime,
                                           NameEmployer = employer.Name,
                                           ChangedData = history.ChangedData
                                       }).OrderBy(h => h.OperationTime).ToList();

                    _historyData.Clear();
                    foreach (var item in historyData)
                    {
                        _historyData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        public void ApplyFilters(List<string> selectedOperationTypes, string searchText)
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    var filteredData = (from history in context.OperationHistory
                                        join employer in context.Employers on history.EmployerId equals employer.IdEmployers
                                        where (selectedOperationTypes.Count == 0 || selectedOperationTypes.Contains(history.OperationType)) &&
                                              (string.IsNullOrEmpty(searchText) ||
                                               history.TableName.Contains(searchText) ||
                                               history.OperationType.Contains(searchText) ||
                                               employer.Name.Contains(searchText) ||
                                               history.ChangedData.Contains(searchText))
                                        select new ClassHistory
                                        {
                                            IdHistory = history.IdHistory,
                                            TableName = history.TableName,
                                            OperationType = history.OperationType,
                                            OperationTime = history.OperationTime,
                                            NameEmployer = employer.Name,
                                            ChangedData = history.ChangedData
                                        }).OrderBy(h => h.OperationTime).ToList();

                    _historyData.Clear();
                    foreach (var item in filteredData)
                    {
                        _historyData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text;
            ApplyFilters(new List<string>(), searchText);
        }


        private void OpenFilterPopup_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            // Сбор выбранных значений из CheckBox'ов
            var selectedOperationTypes = new List<string>();
            if (AddCheckBox.IsChecked == true) selectedOperationTypes.Add("INSERT");
            if (DeleteCheckBox.IsChecked == true) selectedOperationTypes.Add("DELETE");
            if (UpdateCheckBox.IsChecked == true) selectedOperationTypes.Add("UPDATE");

            // Получение текста из текстового поля
            var searchText = SearchTextBox.Text;

            // Применение фильтров
            ApplyFilters(selectedOperationTypes, searchText);

            // Закрытие Popup
            FilterPopup.IsOpen = false;
        }

    }
}


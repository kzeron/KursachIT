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

        /// <summary>
        /// Загрузка данных истории операций.
        /// </summary>
        public void LoadData()
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    var historyData = (from history in context.OperationHistory
                                       join employer in context.Employers on history.UserId equals employer.IdEmployers
                                       select new ClassHistory
                                       {
                                           IdHistory = history.IdHistory,
                                           TableName = history.TableName,
                                           OperationType = history.OperationType,
                                           OperationTime = history.OperationTime,
                                           IdEmployer = employer.Name,
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
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Применение фильтров и поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска.</param>
        public void ApplyFilters(string searchText)
        {
            try
            {
                using (var context = new ITAdminEntities())
                {
                    var filteredData = (from history in context.OperationHistory
                                        join employer in context.Employers on history.UserId equals employer.IdEmployers
                                        where string.IsNullOrEmpty(searchText) ||
                                              history.TableName.Contains(searchText) ||
                                              history.OperationType.Contains(searchText) ||
                                              employer.Name.Contains(searchText) ||
                                              history.ChangedData.Contains(searchText)
                                        select new ClassHistory
                                        {
                                            IdHistory = history.IdHistory,
                                            TableName = history.TableName,
                                            OperationType = history.OperationType,
                                            OperationTime = history.OperationTime,
                                            IdEmployer = employer.Name,
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
            ApplyFilters(SearchTextBox.Text);
        }

        private void OpenFilterPopup_Click(object sender, RoutedEventArgs e)
        {
            // Открытие/закрытие Popup
            FilterPopup.IsOpen = !FilterPopup.IsOpen;
        }
        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текст из поля поиска
            string searchText = SearchTextBox.Text;

            // Применяем фильтры с введенным текстом
            ApplyFilters(searchText);
        }
    }
}

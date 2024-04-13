using System.Data.SqlClient;
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

namespace Аптечный_склад.AdminastratorDB.Pages
{
    /// <summary>
    /// Логика взаимодействия для SeeHistory.xaml
    /// </summary>
    public partial class SeeHistory : Page
    {
        private List<HistoryOperations> historyOperations;
        public SeeHistory()
        {
            InitializeComponent();
            historyOperations = MainWindow.Pharmaceutical_Warehouse.HistoryOperations.ToList();
            LoadOperations();
            dpStart.SelectedDateChanged += UpdateOperations;
            dpEnd.SelectedDateChanged += UpdateOperations;
            tbUserTitle.TextChanged += UpdateOperations;
            tbDetailsOfOperation.TextChanged += UpdateOperations;
        }
        private void LoadOperations()
        {
            // Применяем фильтрацию и сортировку к списку поставок
            var filteredOperations = FilterSupplies(historyOperations);
            var sortedOperations = SortSupplies(filteredOperations);

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgHistory.ItemsSource = sortedOperations;
        }
        private List<HistoryOperations> FilterSupplies(List<HistoryOperations> operations)
        {
            // Применяем фильтры
            var filteredOperations = operations;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredOperations = filteredOperations.Where(op => op.Date >= startDate && op.Date <= endDate).ToList();

            // Фильтрация по имени пользователя
            string userTitleText = tbUserTitle.Text.ToLower();
            if (!string.IsNullOrEmpty(userTitleText))
            {

                filteredOperations = filteredOperations.Where(op => op.User.Title.ToLower().Contains(userTitleText)).ToList();
            }

            // Фильтрация по деталям операции
            string DetailsText = tbDetailsOfOperation.Text.ToLower();
            if (!string.IsNullOrEmpty(DetailsText))
            {

                filteredOperations = filteredOperations.Where(op => op.Details.ToLower().Contains(DetailsText)).ToList();
            }

            return filteredOperations;
        }

        private List<HistoryOperations> SortSupplies(List<HistoryOperations> operations)
        {
            var sortedOperations = operations;


            return sortedOperations;
        }
        private void UpdateOperations(object sender, RoutedEventArgs e)
        {
            LoadOperations();
        }

        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

    }
}

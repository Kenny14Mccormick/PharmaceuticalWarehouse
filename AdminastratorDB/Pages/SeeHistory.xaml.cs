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
        private int currentPageIndex = 0;
        private int itemsPerPage = 15;
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
            ShowCurrentPage();
        }
        private List<HistoryOperations> FilterOperations(List<HistoryOperations> operations)
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

        private void ShowCurrentPage()
        {
            var filteredOperations = FilterOperations(historyOperations);

            dgHistory.ItemsSource = filteredOperations.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (filteredOperations.Count - 1) / itemsPerPage + 1; i++)
            {
                Button pageButton = new Button
                {
                    Content = (i + 1).ToString(),
                    Margin = new Thickness(10, 0, 10, 0),
                    Foreground = new SolidColorBrush(Colors.White)
                };
                if (i == currentPageIndex)
                {
                    pageButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
                }
                else
                {
                    pageButton.ClearValue(Button.BackgroundProperty);
                }
                pageButton.Click += PageButton_Click;
                wpPageNumbers.Children.Add(pageButton);
            }
        }



        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            currentPageIndex = int.Parse(button.Content.ToString()) - 1;
            ShowCurrentPage();
        }


        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            var filteredOperations = FilterOperations(historyOperations);

            if (currentPageIndex < (filteredOperations.Count - 1) / itemsPerPage)
            {
                currentPageIndex++;
                ShowCurrentPage();
            }
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                ShowCurrentPage();
            }
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

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
            "Вы точно хотите очистить записи",
            "Внимание!",
            MessageBoxButton.YesNo,
            MessageBoxImage.Error);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var item in MainWindow.Pharmaceutical_Warehouse.HistoryOperations)
                {
                    MainWindow.Pharmaceutical_Warehouse.HistoryOperations.Remove(item);
                }

                MainWindow.Pharmaceutical_Warehouse.SaveChanges();

                MainWindow.Pharmaceutical_Warehouse.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('HistoryOperations', RESEED, 0);");

                MessageBox.Show("История была успешно очищина");

                dgHistory.ItemsSource = MainWindow.Pharmaceutical_Warehouse.HistoryOperations.ToList();

            }
        }
    }
}

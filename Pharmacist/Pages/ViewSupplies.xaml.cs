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

namespace Аптечный_склад.Pharmacist.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewSupplies.xaml
    /// </summary>
    public partial class ViewSupplies : Page
    {
        private List<PharmacySupply> _supplies; // Список всех поставок
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        public ViewSupplies(List<PharmacySupply> supplies)
        {
            InitializeComponent();
            _supplies = supplies;
            LoadSupplies();
            dpStart.SelectedDateChanged += UpdateSupplies;
            dpEnd.SelectedDateChanged += UpdateSupplies;
            tbSupplyCode.TextChanged += UpdateSupplies; 
        }



        private void LoadSupplies()
        {
            ShowCurrentPage();
        }
        private List<PharmacySupply> FilterSupplies(List<PharmacySupply> supplies)
        {
            // Применяем фильтры
            var filteredSupplies = supplies;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredSupplies = filteredSupplies.Where(supply => supply.Date >= startDate && supply.Date <= endDate).ToList();

            // Фильтрация по номеру поставки
            string supplyCodeText = tbSupplyCode.Text;
            if (!string.IsNullOrEmpty(supplyCodeText))
            {
         
                    filteredSupplies = filteredSupplies.Where(supply => supply.DisplaySupplyCode.Contains(supplyCodeText)).ToList();
                
        
            }

            return filteredSupplies;
        }

        private void ShowCurrentPage()
        {
            var filteredSupplies = FilterSupplies(_supplies);
            dgSupplies.ItemsSource = filteredSupplies.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (filteredSupplies.Count - 1) / itemsPerPage + 1; i++)
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
            var filteredSupplies = FilterSupplies(_supplies);
            if (currentPageIndex < (filteredSupplies.Count - 1) / itemsPerPage)
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


  

        // Обработчик события нажатия кнопки "Подробнее"
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную поставку
            var selectedSupply = dgSupplies.SelectedItem as PharmacySupply;
            if (selectedSupply != null)
            {
                // Создаем страницу с подробной информацией о поставке и передаем выбранную поставку
                Pharmacist.Pages.DetailedSupply detailedSupplyPage = new Pharmacist.Pages.DetailedSupply(selectedSupply, _supplies);
                NavigationService.Navigate(detailedSupplyPage);
            }
        }

        // Метод для обновления списка поставок при изменении фильтров
        private void UpdateSupplies(object sender, RoutedEventArgs e)
        {
            LoadSupplies();
        }

        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }
    }
}

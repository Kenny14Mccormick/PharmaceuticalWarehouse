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

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для SuppliesWireHouse.xaml
    /// </summary>
    public partial class SuppliesWireHouse : Page
    {
        List<MedicineSupply> _supplies;
        int pharmacyManagerCode;
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        public SuppliesWireHouse(int pharmacyManagerCode)
        {
            this.pharmacyManagerCode = pharmacyManagerCode;
            InitializeComponent();
            _supplies = MainWindow.Pharmaceutical_Warehouse.MedicineSupply.ToList();
            var suppliertitle = MainWindow.Pharmaceutical_Warehouse.Supplier.FirstOrDefault(d => d.SupplierCode == 18);
            string stil = suppliertitle.Title;
            ShowCurrentPage();
            dpStart.SelectedDateChanged += UpdateSupplies;
            dpEnd.SelectedDateChanged += UpdateSupplies;
            tbSupplyCode.TextChanged += UpdateSupplies;
        }
        private void UpdateSupplies(object sender, RoutedEventArgs e)
        {
            ShowCurrentPage();

        }



        private List<MedicineSupply> FilterSupplies(List<MedicineSupply> supplies)
        {
            // Применяем фильтры
            var filteredSupplies = supplies;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredSupplies = filteredSupplies.Where(supply => supply.Date >= startDate && supply.Date <= endDate).ToList();

            string supplyCodeText = tbSupplyCode.Text;
            int supplyCode;
            if (!string.IsNullOrEmpty(supplyCodeText) && int.TryParse(supplyCodeText, out supplyCode))
            {
                // supplyCodeText успешно преобразован в int, используйте supplyCode для фильтрации
                filteredSupplies = filteredSupplies.Where(supply => supply.SupplyCode == supplyCode).ToList();
            }
            else
            {
            }

            return filteredSupplies;
        }
        private void ShowCurrentPage()
        {
            var sortedSupplies = FilterSupplies(_supplies);
            dgSupplies.ItemsSource = sortedSupplies.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (sortedSupplies.Count - 1) / itemsPerPage + 1; i++)
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
            var sortedSupplies = FilterSupplies(_supplies);
            if (currentPageIndex < (sortedSupplies.Count - 1) / itemsPerPage)
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
        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную поставку
            var selectedSupply = dgSupplies.SelectedItem as MedicineSupply;
            if (selectedSupply != null)
            {
                // Создаем страницу с подробной информацией о поставке и передаем выбранную поставку
                FolderPharmacyManager.Pages.DetailedWireHouseSupply detailedSupplyPage = new FolderPharmacyManager.Pages.DetailedWireHouseSupply(selectedSupply, _supplies, pharmacyManagerCode);
                NavigationService.Navigate(detailedSupplyPage);
            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SeeMedicine(pharmacyManagerCode));
        }
    }
}

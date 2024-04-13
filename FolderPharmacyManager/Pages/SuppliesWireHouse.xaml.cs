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
        public SuppliesWireHouse(int pharmacyManagerCode)
        {
            this.pharmacyManagerCode = pharmacyManagerCode;
            InitializeComponent();
            _supplies = MainWindow.Pharmaceutical_Warehouse.MedicineSupply.ToList();
            var suppliertitle = MainWindow.Pharmaceutical_Warehouse.Supplier.FirstOrDefault(d => d.SupplierCode == 18);
            string stil = suppliertitle.Title;
            LoadSupplies();
            dpStart.SelectedDateChanged += UpdateSupplies;
            dpEnd.SelectedDateChanged += UpdateSupplies;
            tbSupplyCode.TextChanged += UpdateSupplies;
        }
        private void UpdateSupplies(object sender, RoutedEventArgs e)
        {
            LoadSupplies();
        }

        private void LoadSupplies()
        {
            // Применяем фильтрацию и сортировку к списку поставок
            var sortedSupplies = FilterSupplies(_supplies);

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgSupplies.ItemsSource = sortedSupplies;
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

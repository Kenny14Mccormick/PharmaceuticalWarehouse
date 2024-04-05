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
    /// Логика взаимодействия для ViewSupplies.xaml
    /// </summary>
    public partial class ViewSupplies : Page
    {
        List<PharmacySupply> _supplies;
        public ViewSupplies()
        {
            InitializeComponent();
            _supplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply.OrderBy(x => x.SupplyCode).ToList();

            LoadSupplies();
            dpStart.SelectedDateChanged += UpdateSupplies;
            dpEnd.SelectedDateChanged += UpdateSupplies;
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

        private List<PharmacySupply> FilterSupplies(List<PharmacySupply> supplies)
        {
            // Применяем фильтры
            var filteredSupplies = supplies;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredSupplies = filteredSupplies.Where(supply => supply.Date >= startDate && supply.Date <= endDate).ToList();

            
      

            return filteredSupplies;
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

 

        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void btnCreateDocumnet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

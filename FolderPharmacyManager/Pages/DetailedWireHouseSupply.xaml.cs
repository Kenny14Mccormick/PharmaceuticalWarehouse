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
    /// Логика взаимодействия для DetailedWireHouseSupply.xaml
    /// </summary>
    public partial class DetailedWireHouseSupply : Page
    {

        private List<MedicineSupply> _supplies;
        private MedicineSupply MedicineSupply;
        int pharmacyManagerCode;
            public DetailedWireHouseSupply(MedicineSupply MedicineSupply, List<MedicineSupply> supplies, int pharmacyManagerCode)
        {
            InitializeComponent();
            _supplies = supplies;
            this.pharmacyManagerCode = pharmacyManagerCode;
            this.MedicineSupply = MedicineSupply;
            tblCodeSupply.Text = $"Номер поставки: {MedicineSupply.SupplyCode}";
            tblDateSupply.Text = $"Дата поставки: {MedicineSupply.Date.ToString($"dd'/'MM'/'yyyy")}";
            tblPharmacy.Text = $"Поставщик: {MedicineSupply.Supplier.Title}";
            tblPharmacyManagerSupply.Text = $"Провизор: {MedicineSupply.PharmacyManager.FullName}";
            double totalcost = 0;
            foreach (var content in MedicineSupply.SupplyContent)
            {
                totalcost += content.MedicineTotalCost;
            }
            tblTotalCost.Text = $"Общая сумма: {totalcost} рублей";

            dgDetailedSupply.ItemsSource = MainWindow.Pharmaceutical_Warehouse.SupplyContent.Where(a => a.SupplyCode == MedicineSupply.SupplyCode).ToList();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new FolderPharmacyManager.Pages.SuppliesWireHouse(pharmacyManagerCode));
        }
    }
}

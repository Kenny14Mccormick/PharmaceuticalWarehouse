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
    /// Логика взаимодействия для DetailedSupply.xaml
    /// </summary>
    public partial class DetailedSupply : Page
    {
        private List<PharmacySupply> _supplies;
        private PharmacySupply pharmacySupply;
        
        public DetailedSupply(PharmacySupply pharmacySupply, List<PharmacySupply> supplies)
        {
            InitializeComponent();
            _supplies = supplies;
            this.pharmacySupply = pharmacySupply;
            tblCodeSupply.Text = $"Номер поставки: {pharmacySupply.DisplaySupplyCode}";
            tblDateSupply.Text = $"Дата поставки: {pharmacySupply.Date.ToString($"dd'/'MM'/'yyyy")}";
            tblPharmacyManagerSupply.Text = $"Провизор: {pharmacySupply.PharmacyManager.FullName}";
            double totalcost = 0;
            foreach(var content in pharmacySupply.PharmacySupplyContent)
            {
                totalcost += content.MedicineTotalCost;
            }
            tblTotalCost.Text = $"Общая сумма: {totalcost} рублей";

            dgDetailedSupply.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacySupplyContent.Where(a => a.SupplyCode == pharmacySupply.SupplyCode).ToList();

        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new Pharmacist.Pages.ViewSupplies(_supplies));
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Здесь будет накладная по поставке в WORD)");
        }
    }
}

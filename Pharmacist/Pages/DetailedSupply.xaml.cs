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

        public DetailedSupply(PharmacySupply pharmacySupply, List<PharmacySupply> supplies)
        {
            InitializeComponent();
            _supplies = supplies;
            dgDetailedSupply.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacySupplyContent.Where(a => a.SupplyCode == pharmacySupply.SupplyCode).ToList();

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new Pharmacist.Pages.ViewSupplies(_supplies));
        }
    }
}

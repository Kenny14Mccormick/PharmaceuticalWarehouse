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

namespace Аптечный_склад
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Pharmaceutical_WarehouseEntities Pharmaceutical_Warehouse;

        public MainWindow()
        {
            InitializeComponent();
            Pharmaceutical_Warehouse = new Pharmaceutical_WarehouseEntities();
            LoadApplicationAndSupplyNumbers();
            ShowSplashScreen();

        }

        public void LoadApplicationAndSupplyNumbers()
        {
            var pharmacies = Pharmaceutical_Warehouse.Pharmacy.ToList();
            List<Application> allApplications = new List<Application>();

            // Проходимся по каждой аптеке
            foreach (var pharmacy in pharmacies)
            {
                // Получаем заявки для текущей аптеки
                var pharmacyApplications = MainWindow.Pharmaceutical_Warehouse.Application
                    .Where(app => app.PharmacyCode == pharmacy.PharmacyCode)
                    .ToList();

                // Пронумеруем заявки с единицы
                int applicationNumber = 1;
                foreach (var application in pharmacyApplications)
                {
                    application.DisplayApplicationCode = $"{application.Pharmacy.DisplayDocumentCode}{applicationNumber}";
                    applicationNumber++;
                }

                // Получаем поставки для текущей аптеки
                var pharmacySupplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply
                    .Where(supply => supply.PharmacyCode == pharmacy.PharmacyCode)
                    .ToList();

                // Пронумеруем поставки с единицы
                int supplyNumber = 1;
                foreach (var supply in pharmacySupplies)
                {
                    supply.DisplaySupplyCode = $"{supply.Pharmacy.DisplayDocumentCode}{supplyNumber}";
                    supplyNumber++;
                }
            }
        }

        private async void ShowSplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            Authorization nf = new Authorization();
            nf.Show();
            Hide();
        }
    }
}

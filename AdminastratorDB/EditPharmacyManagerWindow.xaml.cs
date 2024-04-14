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
using System.Windows.Shapes;

namespace Аптечный_склад.AdminastratorDB
{
    /// <summary>
    /// Логика взаимодействия для EditPharmacyManagerWindow.xaml
    /// </summary>
    public partial class EditPharmacyManagerWindow : Window
    {
        private PharmacyManager pharmacyManager;
        public EditPharmacyManagerWindow(PharmacyManager pharmacyManager)
        {
            InitializeComponent();
            this.pharmacyManager = pharmacyManager;
            DataContext = pharmacyManager;
            cbUser.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();
        }
        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (pharmacyManager.PharmacyManagerCode == 0) MainWindow.Pharmaceutical_Warehouse.PharmacyManager.Add(pharmacyManager);
            MainWindow.Pharmaceutical_Warehouse.SaveChanges();

            this.Close();
            MessageBox.Show("Успешно!");
        }
    }
}

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
    /// Логика взаимодействия для SeePharmacies.xaml
    /// </summary>
    public partial class SeePharmacies : Page
    {
        public SeePharmacies()
        {
            InitializeComponent();
            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
            
        }

        private void tbFindPharmacy(object sender, TextChangedEventArgs e)
        {
            string PharmacyTitle = tbPharmacy.Text;
            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy.Where(app => app.Title.ToLower().Contains(PharmacyTitle)).ToList();
        }
        private void tbFindAddress(object sender, TextChangedEventArgs e)
        {
            string address = tbAddress.Text.ToLower();
            string pharmacistName = tbPharmacist.Text.ToLower();

            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy
                .Where(pharmacy => pharmacy.Address.ToLower().Contains(address) &&
                                   pharmacy.PharmacistName.ToLower().Contains(pharmacistName))
                .ToList();
        }

        private void tbFindPharmacist(object sender, TextChangedEventArgs e)
        {
            string address = tbAddress.Text.ToLower();
            string pharmacistName = tbPharmacist.Text.ToLower();

            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy
                .Where(pharmacy => pharmacy.Address.ToLower().Contains(address) &&
                                   pharmacy.PharmacistName.ToLower().Contains(pharmacistName))
                .ToList();
        }


        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            FolderPharmacyManager.AddPharmacy addPharmacy = new AddPharmacy();
            addPharmacy.ShowDialog();
            // Обновляем DataGrid
            dgPharmacies.ItemsSource = null;
            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
        }


        private void btnRemovePharmacy_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную аптеку из DataGrid
            Pharmacy selectedPharmacy = dgPharmacies.SelectedItem as Pharmacy;

            if (selectedPharmacy != null)
            {

                if (selectedPharmacy != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы точно хотите удалить аптеку?",
                        "Подтверждение об удалении",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MainWindow.Pharmaceutical_Warehouse.Pharmacy.Remove(selectedPharmacy);
                        MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                        MessageBox.Show("Запись удалена!");
                        dgPharmacies.ItemsSource = null;
                        dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите аптеку для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

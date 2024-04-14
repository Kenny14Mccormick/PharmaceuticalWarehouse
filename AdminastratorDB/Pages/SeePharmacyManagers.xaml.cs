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

namespace Аптечный_склад.AdminastratorDB.Pages
{
    /// <summary>
    /// Логика взаимодействия для SeePharmacyManagers.xaml
    /// </summary>
    public partial class SeePharmacyManagers : Page
    {
        public SeePharmacyManagers()
        {
            InitializeComponent();
            dgPMs.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.ToList();
        }

        private void btnEditPM_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedPharmacyManager = editButton.DataContext as PharmacyManager;
            AdminastratorDB.EditPharmacyManagerWindow editPharmacyManager = new EditPharmacyManagerWindow(selectedPharmacyManager);
            editPharmacyManager.ShowDialog();
            // Обновляем DataGrid
            dgPMs.ItemsSource = null;
            dgPMs.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.ToList();
        }

        private void btnDeletePM_Click(object sender, RoutedEventArgs e)
        {
            PharmacyManager selectedPharmacyManager = dgPMs.SelectedItem as PharmacyManager;

            if (selectedPharmacyManager != null)
            {

                if (selectedPharmacyManager != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы точно хотите удалить провизора?",
                        "Подтверждение об удалении",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MainWindow.Pharmaceutical_Warehouse.PharmacyManager.Remove(selectedPharmacyManager);
                        MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                        MessageBox.Show("Запись удалена!");
                        dgPMs.ItemsSource = null;
                        dgPMs.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите аптеку для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BddAddPM_Click(object sender, RoutedEventArgs e)
        {
            var pharmacyManager = new PharmacyManager();
            AdminastratorDB.EditPharmacyManagerWindow addPharmacyManager = new EditPharmacyManagerWindow(pharmacyManager);
            addPharmacyManager.ShowDialog();
            // Обновляем DataGrid
            dgPMs.ItemsSource = null;
            dgPMs.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.ToList();
        }
        private void tbFindPM_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbFindPM.Text.ToLower();
            List<PharmacyManager> filteredManagers = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string[] searchTerms = searchText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                filteredManagers = filteredManagers.Where(manager =>
                    searchTerms.Any(term =>
                        manager.FirstName.ToLower().Contains(term) ||
                        manager.LastName.ToLower().Contains(term) ||
                        manager.Patronymic.ToLower().Contains(term)
                    )
                ).ToList();
            }

            dgPMs.ItemsSource = filteredManagers;
        }



    }
}

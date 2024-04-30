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
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        private List<Pharmacy> pharmacies; // Список отфильтрованных лекарств

        public SeePharmacies()
        {
            InitializeComponent();
            pharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
            ShowCurrentPage();
        }

        private void tbFindPharmacy(object sender, TextChangedEventArgs e)
        {
            string PharmacyTitle = tbPharmacy.Text;
            var filteredpharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy.Where(app => app.Title.ToLower().Contains(PharmacyTitle)).ToList();
            pharmacies = filteredpharmacies;
            ShowCurrentPage();
        }
        private void tbFindAddress(object sender, TextChangedEventArgs e)
        {
            string address = tbAddress.Text.ToLower();
            string pharmacistName = tbPharmacist.Text.ToLower();


            var filteredpharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy
                .Where(pharmacy => pharmacy.Address.ToLower().Contains(address) &&
                                   pharmacy.PharmacistName.ToLower().Contains(pharmacistName))
                .ToList();
            pharmacies = filteredpharmacies;
            ShowCurrentPage();
        }

        private void tbFindPharmacist(object sender, TextChangedEventArgs e)
        {
            string address = tbAddress.Text.ToLower();
            string pharmacistName = tbPharmacist.Text.ToLower();

            var filteredpharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy
                .Where(pharmacy => pharmacy.Address.ToLower().Contains(address) &&
                                   pharmacy.PharmacistName.ToLower().Contains(pharmacistName))
                .ToList();
            pharmacies = filteredpharmacies;
            ShowCurrentPage();
        }


        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            var pharmacy = new Pharmacy();
            FolderPharmacyManager.AddPharmacy addPharmacy = new AddPharmacy(pharmacy);
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

        private void btnEditPharmacy_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedPharmacy = editButton.DataContext as Pharmacy;
            FolderPharmacyManager.AddPharmacy addPharmacy = new AddPharmacy(selectedPharmacy);
            addPharmacy.ShowDialog();
            // Обновляем DataGrid
            dgPharmacies.ItemsSource = null;
            dgPharmacies.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
        }

        private void ShowCurrentPage()
        {

            dgPharmacies.ItemsSource = pharmacies.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (pharmacies.Count - 1) / itemsPerPage + 1; i++)
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
            if (currentPageIndex < (pharmacies.Count - 1) / itemsPerPage)
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
    }
}

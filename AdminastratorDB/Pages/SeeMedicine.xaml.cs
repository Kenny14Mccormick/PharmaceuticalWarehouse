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
    /// Логика взаимодействия для SeeMedicine.xaml
    /// </summary>
    public partial class SeeMedicine : Page
    {
        private List<Medicine> _filteredMedicine; // Список отфильтрованных лекарств

        public SeeMedicine()
        {
            InitializeComponent();
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
            InitializeFilters();
            LoadMedicine();
        }

        private void InitializeFilters()
        {
            // Инициализация фильтров
            var categories = MainWindow.Pharmaceutical_Warehouse.MedicineCategory.Select(mc => mc.Title).ToList();
            categories.Insert(0, "Без фильтра");
            cbCategory.ItemsSource = categories;
            cbCategory.SelectedIndex = 0;

            var substances = MainWindow.Pharmaceutical_Warehouse.ActiveSubstance.Select(asb => asb.Title).ToList();
            substances.Insert(0, "Без фильтра");
            cbSubstance.ItemsSource = substances;
            cbSubstance.SelectedIndex = 0;

            var forms = MainWindow.Pharmaceutical_Warehouse.ReleaseForm.Select(rf => rf.Title).ToList();
            forms.Insert(0, "Без фильтра");
            cbForm.ItemsSource = forms;
            cbForm.SelectedIndex = 0;
        }

        private void LoadMedicine()
        {
            _filteredMedicine = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
            ApplyFilters();
        }


        private void ApplyFilters()
        {
            var filteredMedicine = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();

            // Фильтрация по категории
            if (cbCategory.SelectedIndex > 0)
            {
                string selectedCategory = cbCategory.SelectedItem as string;
                filteredMedicine = filteredMedicine.Where(m => m.MedicineCategory.Title == selectedCategory).ToList();
            }

            // Фильтрация по действующему веществу
            if (cbSubstance.SelectedIndex > 0)
            {
                string selectedSubstance = cbSubstance.SelectedItem as string;
                filteredMedicine = filteredMedicine.Where(m => m.ActiveSubstance.Title == selectedSubstance).ToList();
            }

            // Фильтрация по форме выпуска
            if (cbForm.SelectedIndex > 0)
            {
                string selectedForm = cbForm.SelectedItem as string;
                filteredMedicine = filteredMedicine.Where(m => m.ReleaseForm.Title == selectedForm).ToList();
            }

            // Фильтрация по поисковому запросу
            if (!string.IsNullOrWhiteSpace(tbFindMedicine.Text))
            {
                string searchQuery = tbFindMedicine.Text.ToLower();
                filteredMedicine = filteredMedicine.Where(m => m.Title.ToLower().Contains(searchQuery)).ToList();
            }

            _filteredMedicine = filteredMedicine;
            dgMedicines.ItemsSource = _filteredMedicine;
        }

        private void tbFindeMedicicne_tch(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSubstanceChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbFormChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            var medicine = new Medicine();
            AdminastratorDB.EditMedicineWindow EditUser = new EditMedicineWindow(medicine);
            EditUser.ShowDialog();
            // Обновляем DataGrid
            dgMedicines.ItemsSource = null;
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
        }

        private void tbFindPMeidicne(object sender, TextChangedEventArgs e)
        {
            string MedicineTitle = tbFindMedicine.Text;
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.Where(app => app.Title.ToLower().Contains(MedicineTitle)).ToList();
        }

        private void btnEditMedicine_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedMedicine = editButton.DataContext as Medicine;
            AdminastratorDB.EditMedicineWindow EditUser = new EditMedicineWindow(selectedMedicine);
            EditUser.ShowDialog();
            // Обновляем DataGrid
            dgMedicines.ItemsSource = null;
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
        }

        private void btnDeleteMedicine_Click(object sender, RoutedEventArgs e)
        {
            Medicine selectedMedicine = dgMedicines.SelectedItem as Medicine;

            if (selectedMedicine != null)
            {

                if (selectedMedicine != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы точно хотите удалить лекарственное средство?",
                        "Подтверждение об удалении",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MainWindow.Pharmaceutical_Warehouse.Medicine.Remove(selectedMedicine);
                        MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                        MessageBox.Show("Запись удалена!");
                        dgMedicines.ItemsSource = null;
                        dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            Medicine selectedMedicine = (sender as Button)?.DataContext as Medicine;

            if (selectedMedicine != null)
            {
                // Создаем новое окно MedicineMoreInfo и передаем выбранное лекарство
                MedicineMoreInfo moreInfoWindow = new MedicineMoreInfo(selectedMedicine);
                moreInfoWindow.Show();
            }
        }
    }
}

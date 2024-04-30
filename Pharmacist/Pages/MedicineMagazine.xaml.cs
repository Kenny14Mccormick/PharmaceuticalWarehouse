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
    /// Логика взаимодействия для MedicineMagazine.xaml
    /// </summary>
    public partial class MedicineMagazine : Page
    {
        private List<Medicine> _filteredMedicine; // Список отфильтрованных лекарств
        private int currentPageIndex = 0;
        private int itemsPerPage = 9;
        public MedicineMagazine()
        {
            InitializeComponent();
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

            // После применения фильтров проверяем, что текущая страница находится в пределах доступных страниц
            if (currentPageIndex >= (_filteredMedicine.Count - 1) / itemsPerPage)
            {
                // Если текущая страница больше не существует, перекидываем пользователя на первую страницу
                currentPageIndex = 0;
            }

            ShowCurrentPage();
        }

        private void ShowCurrentPage()
        {
            lvMedicine.ItemsSource = _filteredMedicine.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (_filteredMedicine.Count - 1) / itemsPerPage + 1; i++)
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
            if (currentPageIndex < (_filteredMedicine.Count - 1) / itemsPerPage)
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


        private void btnMedicineInfo_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранное лекарство из ListView
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


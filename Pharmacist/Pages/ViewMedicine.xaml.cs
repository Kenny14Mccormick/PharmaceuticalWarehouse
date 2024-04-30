using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Аптечный_склад.Pharmacist.Pages
{
    public partial class ViewMedicine : Page
    {
        private List<Medicine> _filteredMedicine; // Список отфильтрованных лекарств
        private int _medicineCountInOrder; // Счетчик лекарств в заявке
        private List<Medicine> selectedMedicines = new List<Medicine>(); // Создание коллекции выбранных лекарств
        private int pharmacyCode;
        private User user;
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        public ViewMedicine(int pharmacyCode, User user)
        {
            InitializeComponent();
            this.pharmacyCode = pharmacyCode;
            this.user = user;
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
            foreach (var medicine in _filteredMedicine)
            {
                medicine.IsAdded = false; // Устанавливаем IsAdded в false для каждого лекарства
            }
            ApplyFilters();


            _medicineCountInOrder = 0;
            UpdateMedicineCountInOrder();
        }

        private void UpdateMedicineCountInOrder()
        {
            // Обновление отображения счетчика лекарств в заявке
            tbMedicineCountInOrder.Text = $"Добавлено в заявку: {_medicineCountInOrder}";
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
            dgMedicine.ItemsSource = _filteredMedicine.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadMedicine();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                // Получение выбранного лекарства
                Medicine selectedMedicine = (button.DataContext as Medicine);

                // Добавление выбранного лекарства в список
                selectedMedicines.Add(selectedMedicine);

                // Установка свойства IsAdded в true
                selectedMedicine.IsAdded = true;

                // Увеличение счетчика лекарств в заявке и вывод сообщения
                _medicineCountInOrder++;
                UpdateMedicineCountInOrder();

 
                btnCreateApplication.IsEnabled = true;
                MessageBox.Show("Успешно добавлено в заявку");
            }
        }


        private T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T && ((FrameworkElement)child).Name == name)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child, name);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void btnCreateApplication_Click(object sender, RoutedEventArgs e)
        {
            if (_medicineCountInOrder == 0)
            {
                MessageBox.Show("Для оформления заявки необходимо выбрать хотя бы одно лекарство.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CreateApplication createApplicationPage = new CreateApplication(selectedMedicines, pharmacyCode, user);
                NavigationService.Navigate(createApplicationPage);
            }

        }

        private void btnMoreInfo_Click(object sender, RoutedEventArgs e)
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
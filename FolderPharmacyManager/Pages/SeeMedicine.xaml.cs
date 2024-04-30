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
using Excel = Microsoft.Office.Interop.Excel;

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для SeeMedicine.xaml
    /// </summary>
    public partial class SeeMedicine : Page
    {
        private List<Medicine> _filteredMedicine; // Список отфильтрованных лекарств
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        int pharmacyManagerCode;
        public SeeMedicine(int pharmacyManagerCode)
        {
            this.pharmacyManagerCode = pharmacyManagerCode;
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
            dgMedicines.ItemsSource = _filteredMedicine.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

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

        private void btnOrderMedicine_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderMedicines(pharmacyManagerCode));
        }

        private void btnSuppliesHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SuppliesWireHouse(pharmacyManagerCode));

        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            Excel.Worksheet worksheet = workbook.Worksheets[1];

            Excel.Range headerRange = worksheet.Range["A2:B2"];
            Excel.Range DateRange = worksheet.Range["A1:B1"];
            headerRange.Merge();
            DateRange.Merge();
            headerRange.Cells[1, 1].Value = "Остатки лекарств на складе";
            headerRange.Cells[0, 1].Value = $"Дата отчета: {DateTime.Today.ToShortDateString()}";


            headerRange.Font.Italic = true;
            DateRange.Font.Italic = true;

            var allMedicines = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
            int count = allMedicines.Count();
            Excel.Range range = worksheet.Range[$"A1:B{count+3}"];

            worksheet.Cells[3, 1] = "Название лекарства";
            worksheet.Cells[3, 1].Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleSingle; ;
            worksheet.Cells[3, 2] = "Количество (уп.)";
            worksheet.Cells[3, 2].Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleSingle;


            for (int i = 0; i < count; i++)
            {
                worksheet.Cells[i + 4, 1] = allMedicines[i].Title;
                worksheet.Cells[i + 4, 2] = allMedicines[i].MedicineQuantitiy.Quantity;
            }
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            DateRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            range.Font.Name = "Comic Sans MS";
            range.Font.Size = 14;
            range.Font.Color = (int)(68 + 114 * 256 + 196 * 256 * 256);
            headerRange.Font.Color = Excel.XlRgbColor.rgbBlack;
            DateRange.Font.Color = Excel.XlRgbColor.rgbBlack;
            range.EntireRow.AutoFit();
            range.EntireColumn.AutoFit();
            range.Interior.Color = (int)(237 + 237 * 256 + 237 * 256 * 256);
            headerRange.Interior.Color = (int)(221 + 235 * 256 + 247 * 256 * 256);
            DateRange.Interior.Color = (int)(226 + 239 * 256 + 218 * 256 * 256);

            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;   
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }
}
}

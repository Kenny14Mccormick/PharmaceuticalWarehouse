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
    /// Логика взаимодействия для ViewSupplies.xaml
    /// </summary>
    public partial class ViewSupplies : Page
    {
        List<PharmacySupply> _supplies;
        public ViewSupplies()
        {
            InitializeComponent();
            _supplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply.OrderBy(x => x.SupplyCode).ToList();

            LoadSupplies();
            dpStart.SelectedDateChanged += UpdateSupplies;
            dpEnd.SelectedDateChanged += UpdateSupplies;
            tbSupplyCode.TextChanged += UpdateSupplies;
            tbPharmacy.TextChanged += UpdateSupplies;
        }

        private void UpdateSupplies(object sender, RoutedEventArgs e)
        {
            LoadSupplies();
        }

        private void LoadSupplies()
        {
            // Применяем фильтрацию и сортировку к списку поставок
            var sortedSupplies = FilterSupplies(_supplies);

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgSupplies.ItemsSource = sortedSupplies;
        }

        private List<PharmacySupply> FilterSupplies(List<PharmacySupply> supplies)
        {
            // Применяем фильтры
            var filteredSupplies = supplies;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredSupplies = filteredSupplies.Where(supply => supply.Date >= startDate && supply.Date <= endDate).ToList();

            // Фильтрация по номеру поставки
            string supplyCodeText = tbSupplyCode.Text;
            if (!string.IsNullOrEmpty(supplyCodeText))
            {

                    filteredSupplies = filteredSupplies.Where(supply => supply.DisplaySupplyCode.Contains(supplyCodeText)).ToList();

            }

            var pharmacyNameFilter = tbPharmacy.Text.Trim().ToLower();
            // Фильтрация по названию аптеки
            if (!string.IsNullOrEmpty(pharmacyNameFilter))
            {
                filteredSupplies = filteredSupplies.Where(supply =>
                    supply.Pharmacy.Title.ToLower().Contains(pharmacyNameFilter)).ToList();
            }

            return filteredSupplies;
        }




        // Обработчик события нажатия кнопки "Подробнее"
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную поставку
            var selectedSupply = dgSupplies.SelectedItem as PharmacySupply;
            if (selectedSupply != null)
            {
                // Создаем страницу с подробной информацией о поставке и передаем выбранную поставку
                FolderPharmacyManager.Pages.DetailedSupply detailedSupplyPage = new FolderPharmacyManager.Pages.DetailedSupply(selectedSupply, _supplies);
                NavigationService.Navigate(detailedSupplyPage);
            }
        }

 

        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            var allSupplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply.ToList();

            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

            // Определяем текст периода
            string periodText = "";
            if (dpStart.SelectedDate == null && dpEnd.SelectedDate == null)
            {
                periodText = "Поставки за весь период";
            }
            else
            {
                periodText = $"Поставки с {startDate.ToShortDateString()} по {endDate.ToShortDateString()}";
            }


            allSupplies = allSupplies.Where(app =>
        app.Date >= startDate && app.Date <= endDate).ToList();


            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            Excel.Worksheet worksheet = workbook.Worksheets[1];

            Excel.Range headerRange = worksheet.Range["A2:F2"];
            Excel.Range DateRange = worksheet.Range["A1:F1"];
            Excel.Range PeriodRange= worksheet.Range["A3:F3"];
            headerRange.Merge();
            DateRange.Merge();
            PeriodRange.Merge();
            headerRange.Cells[1, 1].Value = "Поставки лекарственных средств в аптеки";
            headerRange.Cells[0, 1].Value = $"Дата отчета: {DateTime.Today.ToShortDateString()}";
            headerRange.Cells[2, 1].Value = periodText;


            headerRange.Font.Italic = true;
            DateRange.Font.Italic = true;


            int count = allSupplies.Count();
            Excel.Range range = worksheet.Range[$"A1:F{count + 4}"];

            Excel.Range headersTableRange = worksheet.Range["A4:F4"];
            worksheet.Cells[4, 1] = "Номер заявки";
            worksheet.Cells[4, 2] = "Дата заявки";
            worksheet.Cells[4, 3] = "Аптека";
            worksheet.Cells[4, 4] = "Адрес";
            worksheet.Cells[4, 5] = "Провизор";
            worksheet.Cells[4, 6] = "Сумма (руб.)";
            headersTableRange.Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleSingle;

            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                worksheet.Cells[i + 5, 1] = allSupplies[i].DisplaySupplyCode;
                worksheet.Cells[i + 5, 2] = allSupplies[i].Date.ToShortDateString();
                worksheet.Cells[i + 5, 3] = allSupplies[i].Pharmacy.Title;
                worksheet.Cells[i + 5, 4] = allSupplies[i].Pharmacy.Address;
                worksheet.Cells[i + 5, 5] = allSupplies[i].PharmacyManager.FullName;
                worksheet.Cells[i + 5, 6] = allSupplies[i].TotalCost;
                sum += allSupplies[i].TotalCost;
            }

            worksheet.Cells[count + 5, 5] = "Общая сумма:";
            worksheet.Cells[count + 5, 5].Font.Color = (int)(68 + 114 * 256 + 196 * 256 * 256);
            worksheet.Cells[count + 5, 5].Font.Size = 14;
            worksheet.Cells[count + 5, 5].Font.Name = "Comic Sans MS"; ;
            worksheet.Cells[count + 5, 5].Interior.Color = Excel.XlRgbColor.rgbAqua;

            worksheet.Cells[count + 5, 6] = sum;
            worksheet.Cells[count + 5, 6].Font.Color = (int)(68 + 114 * 256 + 196 * 256 * 256);
            worksheet.Cells[count + 5, 6].Font.Size = 14;
            worksheet.Cells[count + 5, 6].Font.Name = "Comic Sans MS"; ;
            worksheet.Cells[count + 5, 6].Interior.Color = Excel.XlRgbColor.rgbAqua;



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

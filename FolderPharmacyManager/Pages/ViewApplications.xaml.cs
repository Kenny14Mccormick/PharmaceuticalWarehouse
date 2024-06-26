﻿    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
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
        /// Логика взаимодействия для ViewApplications.xaml
        /// </summary>
        public partial class ViewApplications : Page
        {
            private List<Application> _applications; // Список всех заявок
            private int pharmacyManagerCode;
            private User user;

            private int currentPageIndex = 0;
            private int itemsPerPage = 10;
        public ViewApplications(int pharmacyManagerCode, User user)
            {

                InitializeComponent();
            this.user = user;
                this.pharmacyManagerCode = pharmacyManagerCode;
                _applications = MainWindow.Pharmaceutical_Warehouse.Application.ToList();

            comboBoxStatus.SelectedIndex = 1;
            List<string> statusDescriptions = new List<string>
    {
        "Все статусы",
        "в ожидании",
        "выполнена"
    };
            comboBoxStatus.ItemsSource = statusDescriptions;
            // Устанавливаем обработчики событий для изменения фильтров
            dpStart.SelectedDateChanged += UpdateApplications;
                dpEnd.SelectedDateChanged += UpdateApplications;
                tbPharmacy.TextChanged += UpdateApplications;
                tbApplicationCode.TextChanged += UpdateApplications;
            comboBoxStatus.SelectionChanged += UpdateApplications;
                LoadApplications();
        }

            private void LoadApplications()
            {
                // Применяем фильтрацию и сортировку к списку заявок
                var filteredAndSortedApplications = FilterAndSortApplications(_applications);

                // Подсчитываем общую стоимость каждой заявки
                foreach (var application in filteredAndSortedApplications)
                {
                    double totalCost = 0;
                    foreach (var content in application.ApplicationContent)
                    {
                        totalCost += content.MedicineQuantity * content.Medicine.MedicinePrice.Price;
                    }
                    application.TotalCost = totalCost;
                }

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            ShowCurrentPage();
            }

            private List<Application> FilterAndSortApplications(List<Application> applications)
            {
                var filteredAndSortedApplications = applications;

                // Применяем фильтры
                var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
                var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

                // Получаем текст из TextBox для поиска по названию аптеки
                var pharmacyNameFilter = tbPharmacy.Text.Trim().ToLower();

            var selectedStatus = comboBoxStatus.SelectedItem as string;
            if (comboBoxStatus.SelectedIndex >= 0 && selectedStatus != "Все статусы")
            {
                filteredAndSortedApplications = filteredAndSortedApplications
                    .Where(app => app.ApplicationStatus.StatusDescription == selectedStatus).ToList();
            }
            // Фильтрация по дате
            filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                    app.Date >= startDate && app.Date <= endDate).ToList();

                // Фильтрация по названию аптеки
                if (!string.IsNullOrEmpty(pharmacyNameFilter))
                {
                    filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                        app.Pharmacy.Title.ToLower().Contains(pharmacyNameFilter)).ToList();
                }

                // Фильтрация по номеру заявки
                string appCodeText = tbApplicationCode.Text;
                if (!string.IsNullOrEmpty(appCodeText))
                {

                        filteredAndSortedApplications = filteredAndSortedApplications.Where(app => app.DisplayApplicationCode.Contains(appCodeText)).ToList();
                }

                return filteredAndSortedApplications;
            }

        private void ShowCurrentPage()
        {
            var filteredAndSortedApplications = FilterAndSortApplications(_applications);

            dgApplications.ItemsSource = filteredAndSortedApplications.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (filteredAndSortedApplications.Count - 1) / itemsPerPage + 1; i++)
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
            var filteredAndSortedApplications = FilterAndSortApplications(_applications);

            if (currentPageIndex < (filteredAndSortedApplications.Count - 1) / itemsPerPage)
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

        private void btnMore_Click(object sender, RoutedEventArgs e)
            {
                // Получаем выбранную заявку
                var selectedApplication = dgApplications.SelectedItem as Application;
                if (selectedApplication != null)
                {
                    // Создаем страницу с подробной информацией о заявке и передаем выбранную заявку
                    DetailedApplication detailedPage = new DetailedApplication(selectedApplication, pharmacyManagerCode, user);
                    NavigationService.Navigate(detailedPage);
                }
            }

            // Метод для обновления списка заявок при изменении фильтров
            private void UpdateApplications(object sender, RoutedEventArgs e)
            {

                LoadApplications();
            }

            private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
            {
                dpStart.SelectedDate = null;
                dpEnd.SelectedDate = null;
            }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            var allApplications = MainWindow.Pharmaceutical_Warehouse.Application.Where(ap => ap.StatusCode == 2).ToList();
            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

            // Определяем текст периода
            string periodText = "";
            if (dpStart.SelectedDate == null && dpEnd.SelectedDate == null)
            {
                periodText = "Заявки за весь период";
            }
            else
            {
                periodText = $"Заявки с {startDate.ToShortDateString()} по {endDate.ToShortDateString()}";
            }



            allApplications = allApplications.Where(app =>
        app.Date >= startDate && app.Date <= endDate).ToList();


            // Подсчитываем общую стоимость каждой заявки
            foreach (var application in allApplications)
            {
                double totalCost = 0;
                foreach (var content in application.ApplicationContent)
                {
                    totalCost += content.MedicineQuantity * content.Medicine.MedicinePrice.Price;
                }
                application.TotalCost = totalCost;
            }
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            Excel.Worksheet worksheet = workbook.Worksheets[1];

            Excel.Range DateRange = worksheet.Range["A1:E1"];
            Excel.Range headerRange = worksheet.Range["A2:E2"];
            Excel.Range PeriodRange = worksheet.Range["A3:E3"];
            headerRange.Merge();
            DateRange.Merge();
            PeriodRange.Merge();
            headerRange.Cells[1, 1].Value = "Выполненные заявки";
            headerRange.Cells[0, 1].Value = $"Дата отчета: {DateTime.Today.ToShortDateString()}";
            headerRange.Cells[2, 1].Value = periodText;
            

            headerRange.Font.Italic = true;
            DateRange.Font.Italic = true;

          
            int count = allApplications.Count();
            Excel.Range range = worksheet.Range[$"A1:E{count + 4}"];

            Excel.Range headersTableRange = worksheet.Range["A4:E4"];
            worksheet.Cells[4, 1] = "Номер заявки";
            worksheet.Cells[4, 2] = "Дата заявки";
            worksheet.Cells[4, 3] = "Аптека";
            worksheet.Cells[4, 4] = "Адрес";
            worksheet.Cells[4, 5] = "Сумма (руб.)";
            headersTableRange.Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleSingle;

            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                worksheet.Cells[i + 5, 1] = allApplications[i].DisplayApplicationCode;
                worksheet.Cells[i + 5, 2] = allApplications[i].Date.ToShortDateString();
                worksheet.Cells[i + 5, 3] = allApplications[i].Pharmacy.Title;
                worksheet.Cells[i + 5, 4] = allApplications[i].Pharmacy.Address;
                worksheet.Cells[i + 5, 5] = allApplications[i].TotalCost;
                sum += allApplications[i].TotalCost;
            }
            worksheet.Cells[count + 5, 4] = "Общая сумма:";
            worksheet.Cells[count + 5, 4].Font.Color = (int)(68 + 114 * 256 + 196 * 256 * 256);
            worksheet.Cells[count + 5, 4].Font.Size = 14;
            worksheet.Cells[count + 5, 4].Font.Name = "Comic Sans MS"; ;
            worksheet.Cells[count + 5, 4].Interior.Color = Excel.XlRgbColor.rgbAqua;

            worksheet.Cells[count + 5, 5] = sum;
            worksheet.Cells[count + 5, 5].Font.Color = (int)(68 + 114 * 256 + 196 * 256 * 256);
            worksheet.Cells[count + 5, 5].Font.Size = 14;
            worksheet.Cells[count + 5, 5].Font.Name = "Comic Sans MS"; ;
            worksheet.Cells[count + 5, 5].Interior.Color = Excel.XlRgbColor.rgbAqua;
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
            range.Interior.Color = (int)(237 +   237 * 256 + 237 * 256 * 256);
            headerRange.Interior.Color = (int)(221 + 235 * 256 + 247 * 256 * 256);
            DateRange.Interior.Color = (int)(226 + 239 * 256 + 218 * 256 * 256);

            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }
    }
    }
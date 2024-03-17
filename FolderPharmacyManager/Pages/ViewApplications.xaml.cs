using System;
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

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewApplications.xaml
    /// </summary>
    public partial class ViewApplications : Page
    {
        private List<Application> _applications; // Список всех заявок
        private int pharmacyManagerCode;


        public ViewApplications(int pharmacyManagerCode)
        {
            InitializeComponent();
            this.pharmacyManagerCode = pharmacyManagerCode;
            // Устанавливаем обработчики событий для изменения фильтров
            dpStart.SelectedDateChanged += UpdateApplications;
            dpEnd.SelectedDateChanged += UpdateApplications;
            tbPharmacy.TextChanged += UpdateApplications;
            comboboxDate.SelectionChanged += UpdateApplications;
            _applications = MainWindow.Pharmaceutical_Warehouse.Application.ToList();

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
            // Фильтрация по статусу (невыполненные заявки)
            filteredAndSortedApplications = filteredAndSortedApplications
                .Where(app => app.StatusCode == 1).ToList();
            // Перенумеровываем заявки, начиная с 1
            for (int i = 0; i < filteredAndSortedApplications.Count; i++)
            {
                filteredAndSortedApplications[i].DisplayApplicationCode = i + 1;
            }

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgApplications.ItemsSource = filteredAndSortedApplications;
        }

        private List<Application> FilterAndSortApplications(List<Application> applications)
        {
            var filteredAndSortedApplications = applications;

            // Применяем фильтры
            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

            // Получаем текст из TextBox для поиска по названию аптеки
            var pharmacyNameFilter = tbPharmacy.Text.Trim().ToLower();

            // Фильтрация по дате
            filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                app.Date >= startDate && app.Date <= endDate).ToList();

            // Фильтрация по названию аптеки
            if (!string.IsNullOrEmpty(pharmacyNameFilter))
            {
                filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                    app.Pharmacy.Title.ToLower().Contains(pharmacyNameFilter)).ToList();
            }

            // Применяем сортировку
            switch (comboboxDate.SelectedIndex)
            {
                case 0: // По умолчанию
                    filteredAndSortedApplications = filteredAndSortedApplications.OrderBy(app => app.Date).ToList();
                    break;
                case 1: // По возрастанию
                    filteredAndSortedApplications = filteredAndSortedApplications.OrderBy(app => app.Date).ToList();
                    break;
                case 2: // По убыванию
                    filteredAndSortedApplications = filteredAndSortedApplications.OrderByDescending(app => app.Date).ToList();
                    break;
                default:
                    filteredAndSortedApplications = filteredAndSortedApplications.OrderBy(app => app.Date).ToList();
                    break;
            }

            return filteredAndSortedApplications;
        }


        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную заявку
            var selectedApplication = dgApplications.SelectedItem as Application;
            if (selectedApplication != null)
            {
                // Создаем страницу с подробной информацией о заявке и передаем выбранную заявку
                DetailedApplication detailedPage = new DetailedApplication(selectedApplication, _applications);
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

        private void btnMakeSupply_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную заявку
            var selectedApplication = dgApplications.SelectedItem as Application;
            if (selectedApplication != null)
            {
                using (var dbContext = new Pharmaceutical_WarehouseEntities())
                {
                    bool isEnoughMedicine = true;

                    // Проверяем, достаточно ли всех лекарств на складе
                    foreach (var content in selectedApplication.ApplicationContent)
                    {
                        var medicine = dbContext.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                        if (medicine.MedicineQuantitiy.Quantity < content.MedicineQuantity)
                        {
                            isEnoughMedicine = false;
                            MessageBox.Show($"Недостаточно {medicine.Title} на складе.");
                            break;
                        }
                    }

                    if (isEnoughMedicine)
                    {
                        // Создаем новую поставку
                        var newSupply = new PharmacySupply
                        {
                            Date = DateTime.Now,
                            PharmacyCode = selectedApplication.PharmacyCode,
                            PharmacyManagerCode = pharmacyManagerCode
                        };
                        dbContext.PharmacySupply.Add(newSupply);
                        dbContext.SaveChanges();

                        // Добавляем лекарства из заявки в содержимое поставки
                        foreach (var content in selectedApplication.ApplicationContent)
                        {
                            var newSupplyContent = new PharmacySupplyContent
                            {
                                SupplyCode = newSupply.SupplyCode,
                                MedicineCode = content.MedicineCode,
                                MedicineQuantity = content.MedicineQuantity
                            };
                            var medicine = dbContext.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                            medicine.MedicineQuantitiy.Quantity -= content.MedicineQuantity;
                            dbContext.PharmacySupplyContent.Add(newSupplyContent);
                        }

                        // Получаем заявку из текущего контекста данных
                        var applicationToUpdate = dbContext.Application.FirstOrDefault(a => a.ApplicationCode == selectedApplication.ApplicationCode);

                        if (applicationToUpdate != null)
                        {
                            // Обновляем статус заявки на "Выполнена"
                            applicationToUpdate.StatusCode = 2; // предполагая, что 2 означает "Выполнена"
                            dbContext.SaveChanges();
                        }
                        dgApplications.ItemsSource = dbContext.Application.ToList();
                        LoadApplications();
                        MessageBox.Show("Поставка успешно создана и заявка обновлена на 'Выполнена'!");
                        
                    }
                    else
                    {
                        MessageBox.Show("Поставка не создана из-за недостатка лекарств на складе.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для создания поставки.");
            }
        }






    }
}
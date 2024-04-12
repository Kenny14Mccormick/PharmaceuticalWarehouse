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
            private User user;

            public ViewApplications(int pharmacyManagerCode, List<Application> applications, User user)
            {
                InitializeComponent();
            this.user = user;
                this.pharmacyManagerCode = pharmacyManagerCode;
                _applications = applications;
                // Устанавливаем обработчики событий для изменения фильтров
                dpStart.SelectedDateChanged += UpdateApplications;
                dpEnd.SelectedDateChanged += UpdateApplications;
                tbPharmacy.TextChanged += UpdateApplications;
                tbApplicationCode.TextChanged += UpdateApplications;
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

                // Фильтрация по номеру заявки
                string appCodeText = tbApplicationCode.Text;
                if (!string.IsNullOrEmpty(appCodeText))
                {

                        filteredAndSortedApplications = filteredAndSortedApplications.Where(app => app.DisplayApplicationCode.Contains(appCodeText)).ToList();
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
                    DetailedApplication detailedPage = new DetailedApplication(selectedApplication, _applications, pharmacyManagerCode, user);
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
        }
    }
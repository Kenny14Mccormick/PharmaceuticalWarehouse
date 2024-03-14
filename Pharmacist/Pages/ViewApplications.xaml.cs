using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Аптечный_склад.Pharmacist.Pages
{
    public partial class ViewApplications : Page
    {
        private List<Application> _applications; // Список всех заявок

        public ViewApplications(List<Application> applications)
        {
            InitializeComponent();
            comboboxDate.SelectedIndex = 0;
            comboBoxStatus.SelectedIndex = 0;
            // Создаем коллекцию строк с описанием статусов заявок
            List<string> statusDescriptions = new List<string>
                {
                    "Все статусы",
                    "в ожидании",
                    "выполнена",
                    "отклонена"
                };

            // Устанавливаем список описаний статусов как источник данных для ComboBox
            comboBoxStatus.ItemsSource = statusDescriptions;

            // Устанавливаем обработчики событий для изменения фильтров
            dpStart.SelectedDateChanged += UpdateApplications;
            dpEnd.SelectedDateChanged += UpdateApplications;
            comboBoxStatus.SelectionChanged += UpdateApplications;
            comboboxDate.SelectionChanged += UpdateApplications;

            _applications = applications;
            LoadApplications();
        }
        private void LoadApplications()
        {
            // Применяем фильтрацию и сортировку к списку заявок
            var filteredAndSortedApplications = FilterAndSortApplications(_applications);

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgApplications.ItemsSource = filteredAndSortedApplications;
        }


        private List<Application> FilterAndSortApplications(List<Application> applications)
        {
            var filteredAndSortedApplications = applications;

            // Применяем фильтры
            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

            // Фильтрация по дате
            filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                app.Date >= startDate && app.Date <= endDate).ToList();

            // Фильтрация по статусу (если выбран какой-то статус, кроме "Все статусы")
            var selectedStatus = comboBoxStatus.SelectedItem as string;
            if (comboBoxStatus.SelectedIndex >= 0 && selectedStatus != "Все статусы")
            {
                filteredAndSortedApplications = filteredAndSortedApplications
                    .Where(app => app.ApplicationStatus.StatusDescription == selectedStatus).ToList();
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
    }
}

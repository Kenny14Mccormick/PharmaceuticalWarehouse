﻿using System;
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
            comboBoxStatus.SelectedIndex = 0;
            List<string> statusDescriptions = new List<string>
    {
        "Все статусы",
        "в ожидании",
        "выполнена"
    };
            comboBoxStatus.ItemsSource = statusDescriptions;
            dpStart.SelectedDateChanged += UpdateApplications;
            dpEnd.SelectedDateChanged += UpdateApplications;
            comboBoxStatus.SelectionChanged += UpdateApplications;
            tbApplicationCode.TextChanged += UpdateApplications; // Добавляем обработчик событий для текстового поля
            _applications = applications;
            LoadApplications();
        }

        private List<Application> FilterAndSortApplications(List<Application> applications)
        {
            var filteredAndSortedApplications = applications;
            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            var endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;

            filteredAndSortedApplications = filteredAndSortedApplications.Where(app =>
                app.Date >= startDate && app.Date <= endDate).ToList();

            var selectedStatus = comboBoxStatus.SelectedItem as string;
            if (comboBoxStatus.SelectedIndex >= 0 && selectedStatus != "Все статусы")
            {
                filteredAndSortedApplications = filteredAndSortedApplications
                    .Where(app => app.ApplicationStatus.StatusDescription == selectedStatus).ToList();
            }

            // Фильтрация по номеру заявки
            string applicationCodeText = tbApplicationCode.Text;
            if (!string.IsNullOrEmpty(applicationCodeText))
            {
       
                    filteredAndSortedApplications = filteredAndSortedApplications.Where(application => application.DisplayApplicationCode.Contains(applicationCodeText)).ToList();
            }


            return filteredAndSortedApplications;
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
            dgApplications.ItemsSource = filteredAndSortedApplications.OrderByDescending(a=>a.Date);
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

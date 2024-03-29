﻿using System;
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
    /// Логика взаимодействия для ViewSupplies.xaml
    /// </summary>
    public partial class ViewSupplies : Page
    {
        private List<PharmacySupply> _supplies; // Список всех поставок

        public ViewSupplies(List<PharmacySupply> supplies)
        {
            InitializeComponent();
            _supplies = supplies;
            LoadSupplies();

            dpStart.SelectedDateChanged += UpdateSupplies;

            dpEnd.SelectedDateChanged += UpdateSupplies;

            comboboxDate.SelectionChanged += UpdateSupplies;
        }


        private void LoadSupplies()
        {
            // Применяем фильтрацию и сортировку к списку поставок
            var filteredSupplies = FilterSupplies(_supplies);
            var sortedSupplies = SortSupplies(filteredSupplies);

            // Устанавливаем отфильтрованный и отсортированный список как источник данных для DataGrid
            dgApplications.ItemsSource = sortedSupplies;
        }

        private List<PharmacySupply> FilterSupplies(List<PharmacySupply> supplies)
        {
            // Применяем фильтры
            var filteredSupplies = supplies;

            // Пример фильтрации по дате (если заданы даты начала и конца периода)
            DateTime startDate = dpStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEnd.SelectedDate ?? DateTime.MaxValue;
            filteredSupplies = filteredSupplies.Where(supply => supply.Date >= startDate && supply.Date <= endDate).ToList();

            // Здесь можно добавить другие фильтры по вашему желанию

            return filteredSupplies;
        }

        private List<PharmacySupply> SortSupplies(List<PharmacySupply> supplies)
        {
            var sortedSupplies = supplies;

            // Проверяем выбранный вариант сортировки
            switch (comboboxDate.SelectedIndex)
            {
                case 0: // По умолчанию
                    sortedSupplies = sortedSupplies.OrderBy(supply => supply.Date).ToList();
                    break;
                case 1: // По возрастанию
                    sortedSupplies = sortedSupplies.OrderBy(supply => supply.Date).ToList();
                    break;
                case 2: // По убыванию
                    sortedSupplies = sortedSupplies.OrderByDescending(supply => supply.Date).ToList();
                    break;
            }

            return sortedSupplies;
        }

        // Обработчик события нажатия кнопки "Подробнее"
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную поставку
            var selectedSupply = dgApplications.SelectedItem as PharmacySupply;
            if (selectedSupply != null)
            {
                // Создаем страницу с подробной информацией о поставке и передаем выбранную поставку
                Pharmacist.Pages.DetailedSupply detailedSupplyPage = new Pharmacist.Pages.DetailedSupply(selectedSupply, _supplies);
                NavigationService.Navigate(detailedSupplyPage);
            }
        }

        // Метод для обновления списка поставок при изменении фильтров
        private void UpdateSupplies(object sender, RoutedEventArgs e)
        {
            LoadSupplies();
        }

        private void ResetDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Аптечный_склад.FolderPharmacyManager
{
    /// <summary>
    /// Логика взаимодействия для WindowPharmacyManager.xaml
    /// </summary>
    public partial class WindowPharmacyManager : Window, INotifyPropertyChanged
    {
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public WindowPharmacyManager(User currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;
            DataContext = this; // Устанавливаем текущий объект как контекст данных для окна
        }

        // Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Authorization authorization = new Authorization();
            authorization.Show();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void UserSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            // Создаем объект страницы настроек
            FolderPharmacyManager.Pages.UserSettings userSettings = new FolderPharmacyManager.Pages.UserSettings(CurrentUser);

            // Открываем страницу настроек во фрейме
            MyFrame.NavigationService.Navigate(userSettings);
        }


        private void AddApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            // Получаем аптеку, в которой работает текущий пользователь (фармацевт)
            var pharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.PharmacyCode == CurrentUser.PharmacyCode);

            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            // Передаем номер аптеки при создании объекта страницы ViewMedicine
            MyFrame.NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacy.PharmacyCode));
        }


        private void SeeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            // Создаем объект страницы просмотра заявок
            FolderPharmacyManager.Pages.ViewApplications viewApplications = new Pages.ViewApplications();
            // Открываем страницу просмотра заявок во фрейме
            MyFrame.NavigationService.Navigate(viewApplications);
        }

        private void SeeSuppliesBtn_Click(object sender, RoutedEventArgs e)
        {
            // Изменение внешнего вида кнопок навигации
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            // Получаем текущую аптеку пользователя
            var pharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.PharmacyCode == CurrentUser.PharmacyCode);

            // Получаем поставки только для текущей аптеки
            var pharmacySupplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply
                .Where(supply => supply.PharmacyCode == pharmacy.PharmacyCode)
                .ToList();

            // Пронумеруем поставки с единицы
            int supplyNumber = 1;
            foreach (var supply in pharmacySupplies)
            {
                supply.DisplaySupplyCode = supplyNumber;
                supplyNumber++;
            }
            // Создаем страницу просмотра поставок для текущей аптеки
            //FolderPharmacyManager..ViewSupplies viewSuppliesPage = new FolderPharmacyManager.Pages.ViewSupplies(pharmacySupplies);

            //// Открываем страницу просмотра поставок во фрейме
            //MyFrame.NavigationService.Navigate(viewSuppliesPage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = null;
            // Изменение внешнего вида кнопок навигации
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
        }

        private void SeeMedicinesBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManagePharmaciesBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
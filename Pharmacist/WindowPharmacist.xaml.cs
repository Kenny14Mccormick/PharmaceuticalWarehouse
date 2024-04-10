using System;
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

namespace Аптечный_склад.Pharmacist
{
    /// <summary>
    /// Логика взаимодействия для WindowPharmacist.xaml
    /// </summary>
    public partial class WindowPharmacist : Window, INotifyPropertyChanged
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

        public WindowPharmacist(User currentUser)
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
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            AddApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            MagazineMedicinesBtn.ClearValue(Control.BackgroundProperty);

            // Создаем объект страницы настроек
            Pharmacist.Pages.UserSettings userSettingsPage = new Pharmacist.Pages.UserSettings(CurrentUser);

            // Открываем страницу настроек во фрейме
            MyFrame.NavigationService.Navigate(userSettingsPage);
        }


        private void AddApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            // Получаем аптеку, в которой работает текущий пользователь (фармацевт)
            var pharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.PharmacyCode == CurrentUser.PharmacyCode);

            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            AddApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500")); 
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MagazineMedicinesBtn.ClearValue(Control.BackgroundProperty);

            // Передаем номер аптеки при создании объекта страницы ViewMedicine
            MyFrame.NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacy.PharmacyCode));
        }


        private void SeeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            AddApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MagazineMedicinesBtn.ClearValue(Control.BackgroundProperty);

            // Получаем аптеку, в которой работает текущий пользователь (фармацевт)
            var pharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.PharmacyCode == CurrentUser.PharmacyCode);

            // Получаем заявки только для текущей аптеки
            var pharmacyApplications = MainWindow.Pharmaceutical_Warehouse.Application
                .Where(app => app.PharmacyCode == pharmacy.PharmacyCode)
                .ToList();

            // Создаем объект страницы просмотра заявок
            Pharmacist.Pages.ViewApplications viewApplicationsPage = new Pharmacist.Pages.ViewApplications(pharmacyApplications);

            // Открываем страницу просмотра заявок во фрейме
            MyFrame.NavigationService.Navigate(viewApplicationsPage);
        }

        private void SeeSuppliesBtn_Click(object sender, RoutedEventArgs e)
        {
            // Изменение внешнего вида кнопок навигации
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            AddApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MagazineMedicinesBtn.ClearValue(Control.BackgroundProperty);

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
                supply.DisplaySupplyCode = $"{supply.Pharmacy.DisplayDocumentCode}{supplyNumber}";
                supplyNumber++;
            }
            // Создаем страницу просмотра поставок для текущей аптеки
            Pharmacist.Pages.ViewSupplies viewSuppliesPage = new Pharmacist.Pages.ViewSupplies(pharmacySupplies);

            // Открываем страницу просмотра поставок во фрейме
            MyFrame.NavigationService.Navigate(viewSuppliesPage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = null;
            // Изменение внешнего вида кнопок навигации
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            AddApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MagazineMedicinesBtn.ClearValue(Control.BackgroundProperty);
        }

        private void MagazineMedicinesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            AddApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MagazineMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));

            // Открываем страницу просмотра заявок во фрейме
            MyFrame.NavigationService.Navigate(new Pharmacist.Pages.MedicineMagazine());
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработчик: Степан Хортов Васильевич (НЕ ЗАБУДЬ СДЕЛАТЬ СПРАВКУ)");
        }
    }
}

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
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
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
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            // Передаем номер аптеки при создании объекта страницы ViewMedicine
            MyFrame.NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacy.PharmacyCode));
        }


        private void SeeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            var pharmacyMannagercode = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.FirstOrDefault(p => p.UserCode == CurrentUser.UserCode);
            // Создаем объект страницы просмотра заявок
            Pharmaceutical_WarehouseEntities dbcontext = new Pharmaceutical_WarehouseEntities();
            FolderPharmacyManager.Pages.ViewApplications viewApplications = new Pages.ViewApplications(pharmacyMannagercode.PharmacyManagerCode);
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
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            // Пронумеруем поставки с единицы
            int supplyNumber = 1;
            foreach (var supply in MainWindow.Pharmaceutical_Warehouse.PharmacySupply.ToList())
            {
                supply.DisplaySupplyCode = supplyNumber;
                supplyNumber++;
            }
            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewSupplies());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = null;
            // Изменение внешнего вида кнопок навигации
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
        }

        private void SeeMedicinesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.SeeMedicine());

        }

        private void ManagePharmaciesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.SeePharmacies());

        }
    }
}
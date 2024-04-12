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
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Выход из аккаунта
                this.Close();
                Authorization authorization = new Authorization();
                authorization.Show();

                var historyOperation = new HistoryOperations
                {
                    UserCode = CurrentUser.UserCode,
                    Date = DateTime.Now,
                    Details = "Выход из аккаунта",
                    Type = "Авторизация"
                };
                MainWindow.Pharmaceutical_Warehouse.HistoryOperations.Add(historyOperation);
                MainWindow.Pharmaceutical_Warehouse.SaveChanges();
            }
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
            SeeMedicinesBtn.ClearValue(Control.BackgroundProperty);
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            ManagePharmaciesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            // Создаем объект страницы настроек
            FolderPharmacyManager.Pages.UserSettings userSettings = new FolderPharmacyManager.Pages.UserSettings(CurrentUser);

            // Открываем страницу настроек во фрейме
            MyFrame.NavigationService.Navigate(userSettings);
        }



        private void SeeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeMedicinesBtn.ClearValue(Control.BackgroundProperty);
            SeeApplicationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            ManagePharmaciesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            var pharmacyMannagercode = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.FirstOrDefault(p => p.UserCode == CurrentUser.UserCode);
            var applications = MainWindow.Pharmaceutical_Warehouse.Application.ToList();
      
            // Передаем pharmacyMannagercode.PharmacyManagerCode в конструктор ViewApplications
            FolderPharmacyManager.Pages.ViewApplications viewApplications = new Pages.ViewApplications(pharmacyMannagercode.PharmacyManagerCode, applications, CurrentUser);

            // Открываем страницу просмотра заявок во фрейме
            MyFrame.NavigationService.Navigate(viewApplications);
        }


        private void SeeSuppliesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeMedicinesBtn.ClearValue(Control.BackgroundProperty);
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            ManagePharmaciesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewSupplies());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = null;
            // Изменение внешнего вида кнопок навигации
            SeeMedicinesBtn.ClearValue(Control.BackgroundProperty);
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            ManagePharmaciesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
        }

        private void SeeMedicinesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            ManagePharmaciesBtn.ClearValue(Control.BackgroundProperty);
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);

            var pharmacyMannagercode = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.FirstOrDefault(p => p.UserCode == CurrentUser.UserCode);

            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.SeeMedicine(pharmacyMannagercode.PharmacyManagerCode));

        }

        private void ManagePharmaciesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeMedicinesBtn.ClearValue(Control.BackgroundProperty);
            SeeApplicationBtn.ClearValue(Control.BackgroundProperty);
            SeeSuppliesBtn.ClearValue(Control.BackgroundProperty);
            ManagePharmaciesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            UserSettingsBtn.ClearValue(Control.BackgroundProperty);
            MyFrame.NavigationService.Navigate(new FolderPharmacyManager.Pages.SeePharmacies());

        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Справка");
        }
    }
}
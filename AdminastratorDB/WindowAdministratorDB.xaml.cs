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

namespace Аптечный_склад.AdminastratorDB
{
    /// <summary>
    /// Логика взаимодействия для WindowAdministratorDB.xaml
    /// </summary>
    public partial class WindowAdministratorDB : Window, INotifyPropertyChanged
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

        public WindowAdministratorDB(User currentUser)
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
            SeeUsersBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeHistoryBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            // Создаем объект страницы настроек
            FolderPharmacyManager.Pages.UserSettings userSettings = new FolderPharmacyManager.Pages.UserSettings(CurrentUser);

            // Открываем страницу настроек во фрейме
            MyFrame.NavigationService.Navigate(userSettings);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = null;
            // Изменение внешнего вида кнопок навигации
            SeeUsersBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeHistoryBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
        }

        private void SeeMedicinesBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeUsersBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeHistoryBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            MyFrame.NavigationService.Navigate(new AdminastratorDB.Pages.SeeMedicine());

        }

        private void SeeUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeUsersBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeHistoryBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));

            MyFrame.NavigationService.Navigate(new AdminastratorDB.Pages.SeeUsers());

        }

        private void SeeHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            SeeUsersBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeMedicinesBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            UserSettingsBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F8A9E"));
            SeeHistoryBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            MyFrame.NavigationService.Navigate(new AdminastratorDB.Pages.SeeHistory());
        }
    }
}
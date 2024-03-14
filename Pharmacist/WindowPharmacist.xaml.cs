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
            // Создаем объект страницы настроек
            Pharmacist.Pages.UserSettings userSettingsPage = new Pharmacist.Pages.UserSettings(CurrentUser);

            // Открываем страницу настроек во фрейме
            MyFrame.NavigationService.Navigate(userSettingsPage);
        }


        private void AddApplicationBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SeeApplicationBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SeeSuppliesBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

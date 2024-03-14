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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Аптечный_склад.Pharmacist.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page, INotifyPropertyChanged
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
        public UserSettings(User currentUser)
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

        // Обработчик события нажатия кнопки "Изменить" для имени пользователя
        // Обработчики событий для кнопок изменения и сохранения ника
        private void ChangeTitleButton_Click(object sender, RoutedEventArgs e)
        {
            // Показываем TextBox для ввода нового ника
            tbTitle.Visibility = Visibility.Visible;
            tblTitle.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangeTitleButton.Visibility = Visibility.Collapsed;
            SaveTitleButton.Visibility = Visibility.Visible;
        }

        private void SaveTitleButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового ника и показать TextBlock с текущим ником
            tbTitle.Visibility = Visibility.Collapsed;
            tblTitle.Visibility = Visibility.Visible;

            // Обновляем ник пользователя из TextBox
            CurrentUser.Title = tbTitle.Text;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeTitleButton.Visibility = Visibility.Visible;
            SaveTitleButton.Visibility = Visibility.Collapsed;

            // Сохраняем изменения, если необходимо
            // Pharmaceutical_Warehouse.SaveChanges();
        }

        // Обработчики событий для кнопок изменения и сохранения логина
        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Показываем TextBox для ввода нового логина
            tbLogin.Visibility = Visibility.Visible;
            tblLogin.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangeLoginButton.Visibility = Visibility.Collapsed;
            SaveLoginButton.Visibility = Visibility.Visible;
        }

        private void SaveLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового логина и показать TextBlock с текущим логином
            tbLogin.Visibility = Visibility.Collapsed;
            tblLogin.Visibility = Visibility.Visible;

            // Обновляем логин пользователя из TextBox
            CurrentUser.Login = tbLogin.Text;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeLoginButton.Visibility = Visibility.Visible;
            SaveLoginButton.Visibility = Visibility.Collapsed;

            // Сохраняем изменения, если необходимо
            // Pharmaceutical_Warehouse.SaveChanges();
        }

        // Обработчики событий для кнопок изменения и сохранения пароля
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Показываем TextBox для ввода нового пароля
            tbPassword.Visibility = Visibility.Visible;
            tblPassword.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangePasswordButton.Visibility = Visibility.Collapsed;
            SavePasswordButton.Visibility = Visibility.Visible;
        }

        private void SavePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового пароля и показать TextBlock с текущим паролем
            tbPassword.Visibility = Visibility.Collapsed;
            tblPassword.Visibility = Visibility.Visible;

            // Обновляем пароль пользователя из TextBox
            CurrentUser.Password = tbPassword.Text;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangePasswordButton.Visibility = Visibility.Visible;
            SavePasswordButton.Visibility = Visibility.Collapsed;

            // Сохраняем изменения, если необходимо
            // Pharmaceutical_Warehouse.SaveChanges();
        }

    }
}

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

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page, INotifyPropertyChanged
    {

        public User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _originalTitle;
        private string _originalLogin;
        private string _originalPassword;

        public UserSettings(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();

            var fio = MainWindow.Pharmaceutical_Warehouse.PharmacyManager.Where(x => x.UserCode == currentUser.UserCode).ToList();
            tblName.Text = fio[0].FullName;
            // Сохраняем оригинальные значения
            _originalTitle = currentUser.Title;
            _originalLogin = currentUser.Login;
            _originalPassword = currentUser.Password;

            DataContext = this;
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
            _originalTitle = _currentUser.Title;
            // Показываем TextBox для ввода нового ника
            tbTitle.Visibility = Visibility.Visible;
            tblTitle.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangeTitleButton.Visibility = Visibility.Collapsed;
            SaveTitleButton.Visibility = Visibility.Visible;
            CancelUpdatingTitleButton.Visibility = Visibility.Visible;

        }

        private void SaveTitleButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового ника и показать TextBlock с текущим ником
            tbTitle.Visibility = Visibility.Collapsed;
            tblTitle.Visibility = Visibility.Visible;

            // Обновляем ник пользователя из TextBox
            _currentUser.Title = tbTitle.Text;

            // Сохраняем изменения в базе данных
            try
            {
                // Обновляем объект пользователя в базе данных
                var userToUpdate = MainWindow.Pharmaceutical_Warehouse.User.FirstOrDefault(u => u.UserCode == _currentUser.UserCode);
                if (userToUpdate != null)
                {
                    userToUpdate.Title = _currentUser.Title;
                    MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                }

                // Сообщение об успешном сохранении
                MessageBox.Show("Данные успешно сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обработка ошибок сохранения
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeTitleButton.Visibility = Visibility.Visible;
            SaveTitleButton.Visibility = Visibility.Collapsed;
            CancelUpdatingTitleButton.Visibility = Visibility.Collapsed;
        }


        // Обработчики событий для кнопок изменения и сохранения логина
        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            _originalLogin = _currentUser.Login;
            // Показываем TextBox для ввода нового логина
            tbLogin.Visibility = Visibility.Visible;
            tblLogin.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangeLoginButton.Visibility = Visibility.Collapsed;
            SaveLoginButton.Visibility = Visibility.Visible;
            CancelUpdatingLoginButton.Visibility = Visibility.Visible;

        }

        private void SaveLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового логина и показать TextBlock с текущим логином
            tbLogin.Visibility = Visibility.Collapsed;
            tblLogin.Visibility = Visibility.Visible;

            // Обновляем логин пользователя из TextBox
            _currentUser.Login = tbLogin.Text;

            // Сохраняем изменения в базе данных
            try
            {
                // Обновляем объект пользователя в базе данных
                var userToUpdate = MainWindow.Pharmaceutical_Warehouse.User.FirstOrDefault(u => u.UserCode == _currentUser.UserCode);
                if (userToUpdate != null)
                {
                    userToUpdate.Login = _currentUser.Login;
                    MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                }

                // Сообщение об успешном сохранении
                MessageBox.Show("Данные успешно сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обработка ошибок сохранения
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeLoginButton.Visibility = Visibility.Visible;
            SaveLoginButton.Visibility = Visibility.Collapsed;
            CancelUpdatingLoginButton.Visibility = Visibility.Collapsed;
        }


        // Обработчики событий для кнопок изменения и сохранения пароля
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            _originalPassword = _currentUser.Password;
            // Показываем TextBox для ввода нового пароля
            tbPassword.Visibility = Visibility.Visible;
            tblPassword.Visibility = Visibility.Collapsed;

            // Показываем кнопку "Сохранить", скрываем кнопку "Изменить"
            ChangePasswordButton.Visibility = Visibility.Collapsed;
            SavePasswordButton.Visibility = Visibility.Visible;
            CancelUpdatingPasswordButton.Visibility = Visibility.Visible;
        }

        private void SavePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть TextBox для ввода нового пароля и показать TextBlock с текущим паролем
            tbPassword.Visibility = Visibility.Collapsed;
            tblPassword.Visibility = Visibility.Visible;

            // Обновляем пароль пользователя из TextBox
            _currentUser.Password = tbPassword.Text;

            // Сохраняем изменения в базе данных
            try
            {

                // Обновляем объект пользователя в базе данных
                var userToUpdate = MainWindow.Pharmaceutical_Warehouse.User.FirstOrDefault(u => u.UserCode == _currentUser.UserCode);
                if (userToUpdate != null)
                {
                    userToUpdate.Password = _currentUser.Password;
                    MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                }

                // Сообщение об успешном сохранении
                MessageBox.Show("Данные успешно сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обработка ошибок сохранения
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangePasswordButton.Visibility = Visibility.Visible;
            SavePasswordButton.Visibility = Visibility.Collapsed;
            CancelUpdatingPasswordButton.Visibility = Visibility.Collapsed;

        }

        private void CancelUpdatingPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаем оригинальное значение логина
            _currentUser.Password = _originalPassword;

            // Скрыть TextBox для ввода нового логина и показать TextBlock с текущим логином
            tbPassword.Visibility = Visibility.Collapsed;
            tblPassword.Visibility = Visibility.Visible;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangePasswordButton.Visibility = Visibility.Visible;
            SavePasswordButton.Visibility = Visibility.Collapsed;
            CancelUpdatingPasswordButton.Visibility = Visibility.Collapsed;

            OnPropertyChanged(nameof(_currentUser));

        }

        private void CancelUpdatingLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаем оригинальное значение логина
            _currentUser.Login = _originalLogin;

            // Скрыть TextBox для ввода нового логина и показать TextBlock с текущим логином
            tbLogin.Visibility = Visibility.Collapsed;
            tblLogin.Visibility = Visibility.Visible;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeLoginButton.Visibility = Visibility.Visible;
            SaveLoginButton.Visibility = Visibility.Collapsed;
            CancelUpdatingLoginButton.Visibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(_currentUser));
        }


        private void CancelUpdatingTitleButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаем оригинальное значение логина
            _currentUser.Title = _originalTitle;

            // Скрыть TextBox для ввода нового логина и показать TextBlock с текущим логином
            tbTitle.Visibility = Visibility.Collapsed;
            tblTitle.Visibility = Visibility.Visible;

            // Показываем кнопку "Изменить", скрываем кнопку "Сохранить"
            ChangeTitleButton.Visibility = Visibility.Visible;
            SaveTitleButton.Visibility = Visibility.Collapsed;
            CancelUpdatingTitleButton.Visibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(_currentUser));
        }
    }
}

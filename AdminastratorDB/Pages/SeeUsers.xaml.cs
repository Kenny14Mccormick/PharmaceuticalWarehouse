using System;
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

namespace Аптечный_склад.AdminastratorDB.Pages
{
    /// <summary>
    /// Логика взаимодействия для SeeUsers.xaml
    /// </summary>
    public partial class SeeUsers : Page
    {
        private int currentPageIndex = 0;
        private int itemsPerPage = 10;
        public SeeUsers()
        {
            InitializeComponent();

            cbRole.SelectedIndex = 0;

            List<string> RoleDescription = new List<string>
    {
        "Все роли",
        "Фармацевт",
        "Провизор",
        "Администратор"
    };
            cbRole.ItemsSource = RoleDescription;
            dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();

        }

        private void BddAddUser_Click(object sender, RoutedEventArgs e)
        {
            var user = new User();
            AdminastratorDB.EditUserWindow EditUser = new EditUserWindow(user);
            EditUser.ShowDialog();
            // Обновляем DataGrid
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();
        }

        private void cbRoleChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadUsers();
        }

        private void tbFindUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadUsers();
        }
        private void LoadUsers()
        {
            ShowCurrentPage();
        }

        private List<User> GetFilteredUsers()
        {
            var filteredUsers = MainWindow.Pharmaceutical_Warehouse.User.ToList();

            // Фильтрация по выбранной роли
            string selectedRole = cbRole.SelectedItem as string;
            if (selectedRole != null && selectedRole != "Все роли")
            {
                filteredUsers = filteredUsers.Where(u => u.Role.RoleDescription == selectedRole).ToList();
            }

            // Фильтрация по имени пользователя
            string userName = tbFindUser.Text.ToLower();
            filteredUsers = filteredUsers.Where(u => u.Title.ToLower().Contains(userName)).ToList();

            return filteredUsers;
        }

        private void ShowCurrentPage()
        {
            var filteredUsers = GetFilteredUsers();

            dgUsers.ItemsSource = filteredUsers.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (filteredUsers.Count - 1) / itemsPerPage + 1; i++)
            {
                Button pageButton = new Button
                {
                    Content = (i + 1).ToString(),
                    Margin = new Thickness(10, 0, 10, 0),
                    Foreground = new SolidColorBrush(Colors.White)
                };
                if (i == currentPageIndex)
                {
                    pageButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
                }
                else
                {
                    pageButton.ClearValue(Button.BackgroundProperty);
                }
                pageButton.Click += PageButton_Click;
                wpPageNumbers.Children.Add(pageButton);
            }
        }



        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            currentPageIndex = int.Parse(button.Content.ToString()) - 1;
            ShowCurrentPage();
        }


        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            var filteredUsers = GetFilteredUsers();

            if (currentPageIndex < (filteredUsers.Count - 1) / itemsPerPage)
            {
                currentPageIndex++;
                ShowCurrentPage();
            }
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                ShowCurrentPage();
            }
        }
        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedUser = editButton.DataContext as User;
            AdminastratorDB.EditUserWindow EditUser = new EditUserWindow(selectedUser);
            EditUser.ShowDialog();
            // Обновляем DataGrid
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = dgUsers.SelectedItem as User;

            if (selectedUser != null)
            {

                if (selectedUser != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы точно хотите удалить пользователя?",
                        "Подтверждение об удалении",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MainWindow.Pharmaceutical_Warehouse.User.Remove(selectedUser);
                        MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                        MessageBox.Show("Запись удалена!");
                        dgUsers.ItemsSource = null;
                        dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

  
    }
}

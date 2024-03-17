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
using System.Windows.Shapes;

namespace Аптечный_склад
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
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

        private void btnAutorization_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Password;

            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                var user = dbContext.User.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    // Создаем новую запись в HistoryOperations
                    var historyOperation = new HistoryOperations
                    {
                        UserCode = user.UserCode,
                        Date = DateTime.Now,
                        Details = "Вход в систему",
                        Type = "Авторизация"
                    };
                    dbContext.HistoryOperations.Add(historyOperation);
                    dbContext.SaveChanges();

                    switch (user.RoleCode)
                    {
                        case 1:
                            Close();
                            Pharmacist.WindowPharmacist windowPharmacist = new Pharmacist.WindowPharmacist(user);
                            windowPharmacist.Show();
                            break;
                        case 2:
                            Close();
                            FolderPharmacyManager.WindowPharmacyManager windowPharmacyManager = new FolderPharmacyManager.WindowPharmacyManager(user);
                            windowPharmacyManager.Show();
                            break;
                        case 3:
                            Close();
                            AdminastratorDB.WindowAdministratorDB windowAdministratorDB = new AdminastratorDB.WindowAdministratorDB(user);
                            windowAdministratorDB.Show();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неккоректный логин или пароль!");
                }
            }
        }

    }
}

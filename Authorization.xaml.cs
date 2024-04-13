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
            var pharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
            List<Application> allApplications = new List<Application>();

            // Проходимся по каждой аптеке
            foreach (var pharmacy in pharmacies)
            {
                // Получаем заявки для текущей аптеки
                var pharmacyApplications = MainWindow.Pharmaceutical_Warehouse.Application
                    .Where(app => app.PharmacyCode == pharmacy.PharmacyCode)
                    .ToList();

                // Пронумеруем заявки с единицы
                int applicationNumber = 1;
                foreach (var application in pharmacyApplications)
                {
                    application.DisplayApplicationCode = $"{application.Pharmacy.DisplayDocumentCode}{applicationNumber}";
                    applicationNumber++;
                }

                // Получаем поставки для текущей аптеки
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
            }
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


                var user = MainWindow.Pharmaceutical_Warehouse.User.FirstOrDefault(u => u.Login == login && u.Password == password);

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
                MainWindow.Pharmaceutical_Warehouse.HistoryOperations.Add(historyOperation);
                MainWindow.Pharmaceutical_Warehouse.SaveChanges();

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
        

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Введите логин или пароль для дальнейшей работы");
        }
    }
}

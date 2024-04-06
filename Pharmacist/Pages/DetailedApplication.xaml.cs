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

namespace Аптечный_склад.Pharmacist.Pages
{
    /// <summary>
    /// Логика взаимодействия для DetailedApplication.xaml
    /// </summary>
    public partial class DetailedApplication : Page
    {
        private List<Application> _applications;

        public DetailedApplication(Application selectedApplication, List<Application> applications)
        {
            InitializeComponent();
            _applications = applications;
            tblCodeApplication.Text = $"Номер заявки: {selectedApplication.DisplayApplicationCode}";
            tblDateApplication.Text = $"Дата заявки: {selectedApplication.Date.ToString($"dd'/'MM'/'yyyy")}";
            tblStatusApplication.Text = $"Статус заявки: {selectedApplication.ApplicationStatus.StatusDescription}";
            double totalcost = 0;
            foreach (var content in selectedApplication.ApplicationContent)
            {
                totalcost += content.MedicineTotalCost;
            }
            tblTotalCost.Text = $"Общая сумма: {totalcost} рублей";
            dgDetailedApplication.ItemsSource = MainWindow.Pharmaceutical_Warehouse.ApplicationContent.Where(a => a.ApplicationCode == selectedApplication.ApplicationCode).ToList();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new Pharmacist.Pages.ViewApplications(_applications));
        }

    }
}

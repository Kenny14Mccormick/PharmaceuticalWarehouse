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

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPharmacy.xaml
    /// </summary>
    public partial class AddPharmacy : Page
    {
        public AddPharmacy()
        {
            InitializeComponent();
        }

        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                var newPharmacy = new Pharmacy
                {
                    Title = txtTitle.Text,
                    PharmacistName = txtPharmacistName.Text,
                    PharmacistPhone = txtPharmacistPhone.Text,
                    Address = txtAddress.Text
                };

                dbContext.Pharmacy.Add(newPharmacy);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Аптека успешно добавлена!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

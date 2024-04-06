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

namespace Аптечный_склад.FolderPharmacyManager
{
    /// <summary>
    /// Логика взаимодействия для AddPharmacy.xaml
    /// </summary>
    public partial class AddPharmacy : Window
    {
        public AddPharmacy()
        {
            InitializeComponent();
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                var newPharmacy = new Pharmacy
                {
                    Title = tbPharmacy.Text,
                    PharmacistName = tbPharmacistName.Text,
                    PharmacistPhone = tbPhone.Text,
                    Address = tbPharmacyAddress.Text
                };

                dbContext.Pharmacy.Add(newPharmacy);
                dbContext.SaveChanges();
            }
            this.Close();
            MessageBox.Show("Аптека успешно добавлена!");
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

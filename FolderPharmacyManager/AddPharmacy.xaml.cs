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
        private Pharmacy pharmacy;

        public AddPharmacy(Pharmacy pharmacy)
        {
            InitializeComponent();
            this.pharmacy = pharmacy;
            DataContext = pharmacy;
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                if (pharmacy.PharmacyCode == 0) dbContext.Pharmacy.Add(pharmacy);
                dbContext.SaveChanges();
            }
            this.Close();
            MessageBox.Show("Успешно!");
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

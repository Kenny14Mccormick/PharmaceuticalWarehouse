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
    /// Логика взаимодействия для SeeMedicine.xaml
    /// </summary>
    public partial class SeeMedicine : Page
    {
        public SeeMedicine()
        {
            InitializeComponent();
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.ToList();
        }

        private void BtnAddMedicine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbFindPMeidicne(object sender, TextChangedEventArgs e)
        {
            string MedicineTitle = tbMedicine.Text;
            dgMedicines.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Medicine.Where(app => app.Title.ToLower().Contains(MedicineTitle)).ToList();
        }
    }
}

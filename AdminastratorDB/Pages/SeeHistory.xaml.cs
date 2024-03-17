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
    /// Логика взаимодействия для SeeHistory.xaml
    /// </summary>
    public partial class SeeHistory : Page
    {
        public SeeHistory()
        {
            InitializeComponent();
            dgHistory.ItemsSource = MainWindow.Pharmaceutical_Warehouse.HistoryOperations.ToList();
        }

        private void tbFindPMeidicne(object sender, TextChangedEventArgs e)
        {

        }
    }
}

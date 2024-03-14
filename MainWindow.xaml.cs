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

namespace Аптечный_склад
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Pharmaceutical_WarehouseEntities Pharmaceutical_Warehouse;
        public MainWindow()
        {
            InitializeComponent();
            ShowSplashScreen();
            Pharmaceutical_Warehouse = new Pharmaceutical_WarehouseEntities();
        }

        private async void ShowSplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            Authorization nf = new Authorization();
            nf.Show();
            Hide();
        }
    }
}

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

namespace Аптечный_склад.Pharmacist
{
    /// <summary>
    /// Логика взаимодействия для MedicineMoreInfo.xaml
    /// </summary>
    public partial class MedicineMoreInfo : Window
    {
        public MedicineMoreInfo(Medicine selectedMedicine)
        {
            InitializeComponent();

            // Устанавливаем DataContext на выбранное лекарство
            DataContext = selectedMedicine;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void clostButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

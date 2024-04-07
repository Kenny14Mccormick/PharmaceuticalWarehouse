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

namespace Аптечный_склад.AdminastratorDB
{
    /// <summary>
    /// Логика взаимодействия для WindowEditUser.xaml
    /// </summary>
    public partial class WindowEditUser : Window
    {
        public WindowEditUser()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

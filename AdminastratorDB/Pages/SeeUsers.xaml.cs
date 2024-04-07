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
    /// Логика взаимодействия для SeeUsers.xaml
    /// </summary>
    public partial class SeeUsers : Page
    {
        public SeeUsers()
        {
            InitializeComponent();
            
            foreach(var content in MainWindow.Pharmaceutical_Warehouse.Role)
            {
                cbRole.Items.Add(content.RoleDescription);
            }
            dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.ToList();

        }

        private void BddAddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbFindUser_TextChanged(object sender, TextChangedEventArgs e)
        {
                string UserTitle = tbFindUser.Text;
                dgUsers.ItemsSource = MainWindow.Pharmaceutical_Warehouse.User.Where(app => app.Title.ToLower().Contains(UserTitle)).ToList();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbRoleChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

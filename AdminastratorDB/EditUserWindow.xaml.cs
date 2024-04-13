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
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private User user;
        public EditUserWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            DataContext = user;

            // Установка выбранной роли в комбобоксе
            cbRole.SelectedIndex = user.RoleCode;

            // Добавление ролей в комбобокс
            List<string> RoleDescription = new List<string>
        {
            "Фармацевт",
            "Провизор",
            "Администратор"
        };
            cbRole.ItemsSource = RoleDescription;

            // Если выбранная роль - фармацевт, то показываем комбобокс с аптеками
            if (user.RoleCode == 0)
            {
                cbPharmacy.Visibility = Visibility.Visible;
            }
            else
            {
                cbPharmacy.Visibility = Visibility.Collapsed;
            }

            // Добавление аптек в комбобокс
            foreach (var pharmacy in MainWindow.Pharmaceutical_Warehouse.Pharmacy)
            {
                cbPharmacy.Items.Add(pharmacy.Title);
            }

            // Установка выбранного элемента в ComboBox cbPharmacy на основе PharmacyCode пользователя
            if (user.PharmacyCode.HasValue)
            {
                Pharmacy pharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.PharmacyCode == user.PharmacyCode.Value);
                if (pharmacy != null)
                {
                    cbPharmacy.SelectedItem = pharmacy.Title;
                }
            }
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем свойство PharmacyCode пользователя в соответствии с выбранной аптекой из ComboBox cbPharmacy
            if (cbPharmacy.SelectedItem != null)
            {
                Pharmacy selectedPharmacy = MainWindow.Pharmaceutical_Warehouse.Pharmacy.FirstOrDefault(p => p.Title == cbPharmacy.SelectedItem.ToString());
                if (selectedPharmacy != null)
                {
                    user.PharmacyCode = selectedPharmacy.PharmacyCode;
                }
            }

            // Добавляем пользователя в коллекцию, если это новый пользователь
            if (user.UserCode == 0)
            {
                MainWindow.Pharmaceutical_Warehouse.User.Add(user);
            }

            // Сохраняем изменения в базе данных
            MainWindow.Pharmaceutical_Warehouse.SaveChanges();

            // Закрываем окно и выводим сообщение об успешном сохранении
            this.Close();
            MessageBox.Show("Успешно!");
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbRole_changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbRole.SelectedItem as string == "Фармацевт")
            {
                cbPharmacy.Visibility = Visibility.Visible;
                user.RoleCode = 1; // Установка кода роли "Фармацевт"
            }
            else if (cbRole.SelectedItem as string == "Провизор")
            {
                cbPharmacy.Visibility = Visibility.Collapsed;
                user.RoleCode = 2; // Установка кода роли "Провизор"
            }
            else if (cbRole.SelectedItem as string == "Администратор")
            {
                cbPharmacy.Visibility = Visibility.Collapsed;
                user.RoleCode = 3; // Установка кода роли "Администратор"
            }
        }


    }
}

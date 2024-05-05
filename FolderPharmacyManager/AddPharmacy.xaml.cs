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
            tbDocumentCode.Text = pharmacy.DisplayDocumentCode;
            tbPharmacy.Text = pharmacy.Title;
            tbPharmacistName.Text = pharmacy.PharmacistName;
            tbPhone.Text = pharmacy.PharmacistPhone;
            tbPharmacyAddress.Text = pharmacy.Address;
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddPharmacy_Click(object sender, RoutedEventArgs e)
        {
            // Получение данных из текстовых полей
            string displayDocumentCode = tbDocumentCode.Text;
            string title = tbPharmacy.Text;

            // Проверка наличия пустых полей
            if (string.IsNullOrEmpty(displayDocumentCode) || string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка существования кода аптеки
            if (MainWindow.Pharmaceutical_Warehouse.Pharmacy.Any(p => p.DisplayDocumentCode == displayDocumentCode))
            {
                MessageBox.Show("Введенный код аптеки уже существует!", "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка существования названия аптеки
            if (MainWindow.Pharmaceutical_Warehouse.Pharmacy.Any(p => p.Title == title))
            {
                MessageBox.Show("Аптека с таким названием уже существует!", "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получение остальных данных из текстовых полей
            string pharmacistName = tbPharmacistName.Text;
            string pharmacistPhone = tbPhone.Text;
            string address = tbPharmacyAddress.Text;

            // Установка значений в объект Pharmacy
            pharmacy.DisplayDocumentCode = displayDocumentCode;
            pharmacy.Title = title;
            pharmacy.PharmacistName = pharmacistName;
            pharmacy.PharmacistPhone = pharmacistPhone;
            pharmacy.Address = address;

            // Сохранение изменений
            try
            {
                if (pharmacy.PharmacyCode == 0)
                    MainWindow.Pharmaceutical_Warehouse.Pharmacy.Add(pharmacy);

                MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                this.Close();
                MessageBox.Show("Успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

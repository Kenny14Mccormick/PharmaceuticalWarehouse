using Microsoft.Win32;
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
    /// Логика взаимодействия для EditMedicineWindow.xaml
    /// </summary>
    public partial class EditMedicineWindow : Window
    {
        private Medicine medicine;
        public EditMedicineWindow(Medicine medicine)
        {
            InitializeComponent();
            this.medicine = medicine;
            DataContext = medicine;
            cbCategory.ItemsSource = MainWindow.Pharmaceutical_Warehouse.MedicineCategory.ToList();
            cbSubstance.ItemsSource = MainWindow.Pharmaceutical_Warehouse.ActiveSubstance.ToList();
            cbForm.ItemsSource = MainWindow.Pharmaceutical_Warehouse.ReleaseForm.ToList();
            cbManufacturer.ItemsSource = MainWindow.Pharmaceutical_Warehouse.Manufacturer.ToList();
        }


        private void ChosePhoto_Click(object sender, RoutedEventArgs e)
        {
            // Создаем объект OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Устанавливаем фильтр для диалогового окна, чтобы отображались только изображения
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

            // Открываем диалоговое окно и ждем, пока пользователь выберет файл
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному файлу
                string selectedFilePath = openFileDialog.FileName;

                // Обновляем текстовое поле с путем к выбранному файлу
                tbPhoto.Text = selectedFilePath;

                // Отображаем выбранное изображение в Image элементе
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFilePath);
                bitmap.EndInit();
                photo.Source = bitmap;
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (medicine.MedicineCode == 0)
            {
                MainWindow.Pharmaceutical_Warehouse.Medicine.Add(medicine);

                // Создание новой записи в таблице MedicineQuantity
                MedicineQuantitiy medicineQuantity = new MedicineQuantitiy()
                {
                    Quantity = 0
                };
                MainWindow.Pharmaceutical_Warehouse.MedicineQuantitiy.Add(medicineQuantity);
                MedicinePhoto medicinePhoto = new MedicinePhoto()
                {
                    ImageSource = tbPhoto.Text
                };
                medicine.PhotoCode = medicinePhoto.PhotoCode;
                MainWindow.Pharmaceutical_Warehouse.MedicinePhoto.Add(medicinePhoto);

                // Проверка наличия введенной цены перед созданием записи в таблице MedicinePrice
                if (!string.IsNullOrEmpty(tbPrice.Text))
                {
                    MedicinePrice medicinePrice = new MedicinePrice()
                    {
                        Price = double.Parse(tbPrice.Text)
                    };
                    MainWindow.Pharmaceutical_Warehouse.MedicinePrice.Add(medicinePrice);
                }
            }

            // Сохранение изменений в базе данных
            try
            {
                MainWindow.Pharmaceutical_Warehouse.SaveChanges();
                this.Close();
                MessageBox.Show("Успешно!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

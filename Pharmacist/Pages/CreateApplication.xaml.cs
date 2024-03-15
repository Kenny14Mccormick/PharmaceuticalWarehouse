using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Аптечный_склад.Pharmacist.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateApplication.xaml
    /// </summary>
    public partial class CreateApplication : Page
    {
        private List<Medicine> selectedMedicinesBackup;

        private List<Medicine> selectedMedicines;
        Medicine medicine1 = new Medicine();
        private int pharmacyCode;
        private double sum;

        public CreateApplication(List<Medicine> selectedMedicines, int pharmacyCode)
        {
            InitializeComponent();
            this.selectedMedicines = selectedMedicines;
            this.pharmacyCode = pharmacyCode;

            lvSelectedMedicine.ItemsSource = selectedMedicines;

            foreach (var s in selectedMedicines)
            {
                sum += s.MedicinePrice.Price;
            }

            tblSum.Text = $"Итого: {sum} рублей";

            // Добавьте этот код
            foreach (var item in selectedMedicines)
            {
                item.PropertyChanged += MedicinePropertyChanged;
            }

        }

        private void UpdateMedicineQuantity(Medicine medicine)
        {
            // Найти соответствующий объект Medicine в коллекции selectedMedicines
            Medicine selectedMedicine = selectedMedicines.FirstOrDefault(m => m.MedicineCode == medicine.MedicineCode);
            if (selectedMedicine != null)
            {
                selectedMedicine.Quantity = medicine.Quantity;
            }
        }

        private void btnDecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Medicine medicine = (Medicine)btn.DataContext;

            if (medicine.Quantity > 0)
            {
                medicine.Quantity--;
                UpdateTotalSum();
                var item = lvSelectedMedicine.ItemContainerGenerator.ContainerFromItem(medicine) as ListViewItem;
                var textBox = FindVisualChild<TextBox>(item);
                textBox.Text = medicine.Quantity.ToString();
            }
        }

        private void btnIncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Medicine medicine = (Medicine)btn.DataContext;

            medicine.Quantity++;
            UpdateTotalSum();
            var item = lvSelectedMedicine.ItemContainerGenerator.ContainerFromItem(medicine) as ListViewItem;
            var textBox = FindVisualChild<TextBox>(item);
            textBox.Text = medicine.Quantity.ToString();
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }



        private void tbQuantity_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Medicine medicine = (Medicine)textBox.DataContext;

            if (int.TryParse(textBox.Text, out int quantity))
            {
                medicine.Quantity = quantity;
                UpdateTotalSum();
            }
        }


        private void UpdateTotalSum()
        {
            sum = selectedMedicines.Sum(m => m.MedicinePrice.Price * m.Quantity);
            tblSum.Text = $"Итого: {sum} рублей";
        }

        private void MedicinePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                UpdateTotalSum();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            selectedMedicinesBackup = new List<Medicine>(selectedMedicines);
            NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacyCode));
        }

        private void btnOrderMedicine_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, есть ли у пользователя выбранные лекарства
            if (selectedMedicines.Count == 0)
            {
                MessageBox.Show("Для создания заявки необходимо выбрать хотя бы одно лекарство.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Прерываем создание заявки
            }

            // Создание новой заявки
            Application newApplication = new Application
            {
                PharmacyCode = pharmacyCode,
                Date = DateTime.Today,
                PharmacyManagerCode = 1,
                StatusCode = 1
            };

            // Добавление заявки в таблицу Application
            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                dbContext.Application.Add(newApplication);
                dbContext.SaveChanges();
            }

            // Получение кода только что созданной заявки
            int newApplicationCode = newApplication.ApplicationCode;

            // Создание объектов ApplicationContent для каждого выбранного лекарства и их количества
            List<ApplicationContent> applicationContents = new List<ApplicationContent>();
            foreach (var medicine in selectedMedicines)
            {
                ApplicationContent content = new ApplicationContent
                {
                    ApplicationCode = newApplicationCode,
                    MedicineCode = medicine.MedicineCode,
                    MedicineQuantity = medicine.Quantity
                };
                applicationContents.Add(content);
            }

            // Добавление объектов ApplicationContent в таблицу ApplicationContent
            using (var dbContext = new Pharmaceutical_WarehouseEntities())
            {
                dbContext.ApplicationContent.AddRange(applicationContents);
                dbContext.SaveChanges();
            }

            // Оповещение пользователя о успешном создании заявки
            MessageBox.Show("Заявка успешно создана и отправлена!");
        }


        private void btnRemoveMedicine_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Medicine medicine = (Medicine)btn.DataContext;

            // Подтверждение удаления лекарства
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить {medicine.Title}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                medicine.Quantity = 1;
                // Удаление выбранного лекарства из списка
                selectedMedicines.Remove(medicine);

                // Обновление отображения списка лекарств в ListView
                lvSelectedMedicine.ItemsSource = null;
                lvSelectedMedicine.ItemsSource = selectedMedicines;

                // Обновление суммы заказа
                UpdateTotalSum();
            }
        }


    }
}
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

        private List<Medicine> selectedMedicines;
        private int pharmacyCode;
        private double sum;
        private User user;
        public CreateApplication(List<Medicine> selectedMedicines, int pharmacyCode, User user)
        {
            InitializeComponent();
            this.selectedMedicines = selectedMedicines;
            this.pharmacyCode = pharmacyCode;
            this.user = user;
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
            foreach(var a in selectedMedicines)
            {
                a.Quantity = 1;
            }
            NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacyCode, user));
        }

        private void btnOrderMedicine_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, есть ли у пользователя выбранные лекарства
            if (selectedMedicines.Count == 0)
            {
                MessageBox.Show("Для создания заявки необходимо выбрать хотя бы одно лекарство.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Прерываем создание заявки
            }

            // Вычисление общей стоимости заявки
            double totalCost = selectedMedicines.Sum(m => m.MedicinePrice.Price * m.Quantity);

            // Проверка, что общая сумма не равна 0
            if (totalCost == 0)
            {
                MessageBox.Show("Общая сумма заказа равна 0. Добавьте лекарства в заявку перед созданием.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Прерываем создание заявки
            }

            // Создание новой заявки
            Application newApplication = new Application
            {
                PharmacyCode = pharmacyCode,
                Date = DateTime.Today,
                StatusCode = 1,
                TotalCost = totalCost // Присвоение общей стоимости новой заявке
            };

            // Добавление заявки в таблицу Application

            MainWindow.Pharmaceutical_Warehouse.Application.Add(newApplication);
            MainWindow.Pharmaceutical_Warehouse.SaveChanges();
            

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

            MainWindow.Pharmaceutical_Warehouse.ApplicationContent.AddRange(applicationContents);
            MainWindow.Pharmaceutical_Warehouse.SaveChanges();
            

            // Оповещение пользователя о успешном создании заявки
            MessageBox.Show("Заявка успешно создана и отправлена!");
            var historyOperation = new HistoryOperations
            {
                UserCode = user.UserCode,
                Date = DateTime.Now,
                Details = "Создание заявки",
                Type = "Операция"
            };
            MainWindow.Pharmaceutical_Warehouse.HistoryOperations.Add(historyOperation);
            MainWindow.Pharmaceutical_Warehouse.SaveChanges();
            var pharmacies = MainWindow.Pharmaceutical_Warehouse.Pharmacy.ToList();
            List<Application> allApplications = new List<Application>();

            // Проходимся по каждой аптеке
            foreach (var pharmacy in pharmacies)
            {
                // Получаем заявки для текущей аптеки
                var pharmacyApplications = MainWindow.Pharmaceutical_Warehouse.Application
                    .Where(app => app.PharmacyCode == pharmacy.PharmacyCode)
                    .ToList();

                // Пронумеруем заявки с единицы
                int applicationNumber = 1;
                foreach (var application in pharmacyApplications)
                {
                    application.DisplayApplicationCode = $"{application.Pharmacy.DisplayDocumentCode}{applicationNumber}";
                    applicationNumber++;
                }

                // Получаем поставки для текущей аптеки
                var pharmacySupplies = MainWindow.Pharmaceutical_Warehouse.PharmacySupply
                    .Where(supply => supply.PharmacyCode == pharmacy.PharmacyCode)
                    .ToList();

                // Пронумеруем поставки с единицы
                int supplyNumber = 1;
                foreach (var supply in pharmacySupplies)
                {
                    supply.DisplaySupplyCode = $"{supply.Pharmacy.DisplayDocumentCode}{supplyNumber}";
                    supplyNumber++;
                }
            }
            NavigationService.Navigate(new Pharmacist.Pages.ViewMedicine(pharmacyCode, user));
        }



        private void btnRemoveMedicine_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Medicine medicine = (Medicine)btn.DataContext;

            // Подтверждение удаления лекарства
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите убрать {medicine.Title} из списка?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
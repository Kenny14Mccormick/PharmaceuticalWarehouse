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

namespace Аптечный_склад.FolderPharmacyManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для DetailedApplication.xaml
    /// </summary>
    public partial class DetailedApplication : Page
    {
        private List<Application> _applications;
        private User user;
        private Application selectedApplication;
        int pharmacyManagerCode;

        public DetailedApplication(Application selectedApplication, int pharmacyManagerCode, User user)
        {
            InitializeComponent();
            this.user = user;
            _applications = MainWindow.Pharmaceutical_Warehouse.Application.ToList();
            double totalCost = 0;
            this.pharmacyManagerCode = pharmacyManagerCode;
            this.selectedApplication = selectedApplication;
            tblDate.Text = $"Дата заявки: {selectedApplication.Date.ToShortDateString()}";
            tblPharmacy.Text = $"Аптека: {selectedApplication.Pharmacy.Title}";
            tblApplicationCode.Text = $"Номер поставки: {selectedApplication.DisplayApplicationCode}";

            // Проходимся по всем позициям в заявке и вычисляем общую стоимость
            foreach (var content in selectedApplication.ApplicationContent)
            {
                totalCost += content.MedicineQuantity * content.Medicine.MedicinePrice.Price;
            }

            // Устанавливаем текст с общей стоимостью в TextBlock
            tblTotalCost.Text = $"Общая стоимость: {totalCost.ToString("F2")} руб.";
            dgDetailedApplication.ItemsSource = MainWindow.Pharmaceutical_Warehouse.ApplicationContent.Where(a => a.ApplicationCode == selectedApplication.ApplicationCode).ToList();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewApplications(pharmacyManagerCode,  user));
        }


        private void btnMakeSupply_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную заявку
            if (selectedApplication != null)
            {

                    bool isEnoughMedicine = true;

                    // Проверяем, достаточно ли всех лекарств на складе
                    foreach (var content in selectedApplication.ApplicationContent)
                    {
                        var medicine = MainWindow.Pharmaceutical_Warehouse.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                        var medicineQuantity = MainWindow.Pharmaceutical_Warehouse.MedicineQuantitiy.FirstOrDefault(mq => mq.QuantityCode == medicine.QuantityCode);

                        if (medicineQuantity != null)
                        {
                            if (medicineQuantity.Quantity < content.MedicineQuantity)
                            {
                                isEnoughMedicine = false;
                                MessageBox.Show($"Недостаточно {medicine.Title} на складе.");
                                break;
                            }
                        }
                        else
                        {
                            isEnoughMedicine = false;
                            MessageBox.Show($"Лекарство {medicine.Title} отсутствует на складе.");
                            break;
                        }
                    }

                    if (isEnoughMedicine)
                    {
                        // Проверяем, была ли уже создана поставка для этой заявки
                        var existingSupply = MainWindow.Pharmaceutical_Warehouse.PharmacySupply.FirstOrDefault(supply => supply.Application.Any(app => app.ApplicationCode == selectedApplication.ApplicationCode));

                        if (existingSupply == null)
                        {
                            // Создаем новую поставку
                            var newSupply = new PharmacySupply
                            {
                                Date = DateTime.Now,
                                PharmacyCode = selectedApplication.PharmacyCode,
                                PharmacyManagerCode = pharmacyManagerCode
                            };
                        MainWindow.Pharmaceutical_Warehouse.PharmacySupply.Add(newSupply);

                            // Добавляем лекарства из заявки в содержимое поставки
                            foreach (var content in selectedApplication.ApplicationContent)
                            {
                                var newSupplyContent = new PharmacySupplyContent
                                {
                                    SupplyCode = newSupply.SupplyCode,
                                    MedicineCode = content.MedicineCode,
                                    MedicineQuantity = content.MedicineQuantity
                                };
                            MainWindow.Pharmaceutical_Warehouse.PharmacySupplyContent.Add(newSupplyContent);

                                // Обновляем количество лекарств на складе
                                var medicine = MainWindow.Pharmaceutical_Warehouse.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                                var medicineQuantity = MainWindow.Pharmaceutical_Warehouse.MedicineQuantitiy.FirstOrDefault(mq => mq.QuantityCode == medicine.QuantityCode);
                                medicineQuantity.Quantity -= content.MedicineQuantity;
                            }

                            // Обновляем статус заявки на "Выполнена"
                            var applicationToUpdate = MainWindow.Pharmaceutical_Warehouse.Application.FirstOrDefault(app => app.ApplicationCode == selectedApplication.ApplicationCode);
                            if (applicationToUpdate != null)
                            {
                                applicationToUpdate.StatusCode = 2;
                            }

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
                            NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewApplications(pharmacyManagerCode, user));
                            MessageBox.Show("Поставка успешно создана и заявка обновлена на 'Выполнена'!");

                            var historyOperation = new HistoryOperations
                            {
                                UserCode = user.UserCode,
                                Date = DateTime.Now,
                                Details = "Создание поставки",
                                Type = "Операция"
                            };
                        MainWindow.Pharmaceutical_Warehouse.HistoryOperations.Add(historyOperation);
                        MainWindow.Pharmaceutical_Warehouse.SaveChanges();

                        }
                        else
                        {
                            MessageBox.Show("Для этой заявки уже создана поставка.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поставка не создана из-за недостатка лекарств на складе.");
                    }
                }
            
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для создания поставки.");
            }
        }



    }
}

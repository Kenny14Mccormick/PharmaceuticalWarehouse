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

        private Application selectedApplication;
        int pharmacyManagerCode;

        public DetailedApplication(Application selectedApplication, List<Application> applications, int pharmacyManagerCode)
        {
            InitializeComponent();
            _applications = applications;
            double totalCost = 0;
            this.pharmacyManagerCode = pharmacyManagerCode;
            this.selectedApplication = selectedApplication;
            tblDate.Text = $"Дата заявки: {selectedApplication.Date.ToShortDateString()}";
            tblPharmacy.Text = $"Аптека: {selectedApplication.Pharmacy.Title}";

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
            NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewApplications(pharmacyManagerCode, _applications));
        }


        private void btnMakeSupply_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную заявку
            if (selectedApplication != null)
            {
                using (var dbContext = new Pharmaceutical_WarehouseEntities())
                {
                    bool isEnoughMedicine = true;
                    
                    // Проверяем, достаточно ли всех лекарств на складе
                    foreach (var content in selectedApplication.ApplicationContent)
                    {
                        var medicine = dbContext.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                        if (medicine.MedicineQuantitiy.Quantity < content.MedicineQuantity)
                        {
                            isEnoughMedicine = false;
                            MessageBox.Show($"Недостаточно {medicine.Title} на складе.");
                            break;
                        }
                    }

                    if (isEnoughMedicine)
                    {
                        // Создаем новую поставку
                        var newSupply = new PharmacySupply
                        {
                            Date = DateTime.Now,
                            PharmacyCode = selectedApplication.PharmacyCode,
                            PharmacyManagerCode = pharmacyManagerCode
                        };
                        dbContext.PharmacySupply.Add(newSupply);
                        dbContext.SaveChanges();

                        // Добавляем лекарства из заявки в содержимое поставки
                        foreach (var content in selectedApplication.ApplicationContent)
                        {
                            var newSupplyContent = new PharmacySupplyContent
                            {
                                SupplyCode = newSupply.SupplyCode,
                                MedicineCode = content.MedicineCode,
                                MedicineQuantity = content.MedicineQuantity
                            };
                            var medicine = dbContext.Medicine.FirstOrDefault(o => o.MedicineCode == content.MedicineCode);
                            medicine.MedicineQuantitiy.Quantity -= content.MedicineQuantity;
                            dbContext.PharmacySupplyContent.Add(newSupplyContent);
                        }

                        // Получаем заявку из текущего контекста данных
                        var applicationToUpdate = dbContext.Application.FirstOrDefault(a => a.ApplicationCode == selectedApplication.ApplicationCode);

                        if (applicationToUpdate != null)
                        {
                            // Обновляем статус заявки на "Выполнена"
                            applicationToUpdate.StatusCode = 2; // предполагая, что 2 означает "Выполнена"
                            dbContext.SaveChanges();

                            // Обновляем соответствующий элемент в списке _applications
                            var updatedApplicationIndex = _applications.FindIndex(app => app.ApplicationCode == selectedApplication.ApplicationCode);
                            if (updatedApplicationIndex != -1)
                            {
                                _applications[updatedApplicationIndex] = applicationToUpdate;
                            }
                            // Здесь вместо вызова метода LoadApplications() мы передаем список заявок обратно на страницу ViewApplications
                            NavigationService.Navigate(new FolderPharmacyManager.Pages.ViewApplications(pharmacyManagerCode, _applications));
                        }
                        //dgApplications.ItemsSource = dbContext.Application.ToList();
                        //LoadApplications();
                        MessageBox.Show("Поставка успешно создана и заявка обновлена на 'Выполнена'!");

                    }
                    else
                    {
                        MessageBox.Show("Поставка не создана из-за недостатка лекарств на складе.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для создания поставки.");
            }
        }
    }
}

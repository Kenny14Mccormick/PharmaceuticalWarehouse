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

using Word = Microsoft.Office.Interop.Word;
namespace Аптечный_склад.Pharmacist.Pages
{
    /// <summary>
    /// Логика взаимодействия для DetailedSupply.xaml
    /// </summary>
    public partial class DetailedSupply : Page
    {
        private List<PharmacySupply> _supplies;
        private PharmacySupply pharmacySupply;
        double totalcost;
        
        public DetailedSupply(PharmacySupply pharmacySupply, List<PharmacySupply> supplies)
        {
            InitializeComponent();
            _supplies = supplies;
            this.pharmacySupply = pharmacySupply;
            tblCodeSupply.Text = $"Номер поставки: {pharmacySupply.DisplaySupplyCode}";
            tblDateSupply.Text = $"Дата поставки: {pharmacySupply.Date.ToString($"dd'/'MM'/'yyyy")}";
            tblPharmacyManagerSupply.Text = $"Провизор: {pharmacySupply.PharmacyManager.FullName}";
            foreach(var content in pharmacySupply.PharmacySupplyContent)
            {
                totalcost += content.MedicineTotalCost;
            }
            tblTotalCost.Text = $"Общая сумма: {totalcost} рублей";

            dgDetailedSupply.ItemsSource = MainWindow.Pharmaceutical_Warehouse.PharmacySupplyContent.Where(a => a.SupplyCode == pharmacySupply.SupplyCode).ToList();

        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на страницу с просмотром всех заявок и передаем список заявок
            NavigationService.Navigate(new Pharmacist.Pages.ViewSupplies(_supplies));
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            Word.Application app = new Word.Application();
            Word.Document document = app.Documents.Add(@"C:\Users\khort\source\repos\Семестр_2\Аптечный_склад\Накладная71а.doc");

            document.Bookmarks["Номер_накл"].Range.Text = pharmacySupply.DisplaySupplyCode;
            document.Bookmarks["Дата"].Range.Text = pharmacySupply.Date.ToShortDateString();
            document.Bookmarks["Отправитель"].Range.Text = "Аптечный склад";
            document.Bookmarks["Вид_отправителя"].Range.Text = "Распределение лекарств";
            document.Bookmarks["Получатель"].Range.Text = $"Аптека {pharmacySupply.Pharmacy.Title}";
            document.Bookmarks["Вид_получателя"].Range.Text = "Реализация лекарственных средств";
            document.Bookmarks["Основание"].Range.Text = $"приказ от {pharmacySupply.Date.ToShortDateString()} г";
            document.Bookmarks["Кому"].Range.Text = $"аптека";
            document.Bookmarks["Через_кого"].Range.Text = $"Фармацевт {pharmacySupply.PharmacyManager.FullNameDoc}";


            var supplycontent = MainWindow.Pharmaceutical_Warehouse.PharmacySupplyContent.Where(a => a.SupplyCode == pharmacySupply.SupplyCode).ToList();

            Word.Table table = document.Tables[4];

            Word.Range cellRange;
            for (int i = 0; i < supplycontent.Count; i++)
            {
                // Добавление строк в таблицу
                table.Rows.Add();

                cellRange = table.Cell(i + 4, 1).Range;
                cellRange.Text = supplycontent[i].Medicine.Title;

                cellRange = table.Cell(i + 4, 2).Range;
                cellRange.Text = supplycontent[i].Medicine.MedicineCode.ToString();

                cellRange = table.Cell(i + 4, 3).Range;
                cellRange.Text = "упаковка";

                cellRange = table.Cell(i + 4, 4).Range;
                cellRange.Text = supplycontent[i].MedicineQuantity.ToString();

                cellRange = table.Cell(i + 4, 5).Range;
                cellRange.Text = supplycontent[i].MedicineQuantity.ToString();

                cellRange = table.Cell(i + 4, 6).Range;
                cellRange.Text = supplycontent[i].Medicine.MedicinePrice.Price.ToString();

                cellRange = table.Cell(i + 4, 7).Range;
                cellRange.Text = supplycontent[i].MedicineTotalCost.ToString();
            }

            document.Bookmarks["Всего_отпущено"].Range.Text = supplycontent.Count().ToString();
            document.Bookmarks["На_сумму"].Range.Text = totalcost.ToString();
            document.Bookmarks["Разрешил"].Range.Text = pharmacySupply.PharmacyManager.FullNameDoc;
            document.Bookmarks["Отпустил"].Range.Text = pharmacySupply.PharmacyManager.FullNameDoc;
            document.Bookmarks["Подпись_фармацевт"].Range.Text = pharmacySupply.Pharmacy.PharmacistNameDoc;

            app.Visible = true;
        }
    }
}

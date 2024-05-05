using System.Collections.Generic;
using System.ComponentModel;

namespace Аптечный_склад
{
    public partial class MedicineQuantitiy : INotifyPropertyChanged
    {
        public int QuantityCode { get; set; }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    NotifyPropertyChanged(nameof(Quantity));
                    NotifyPropertyChanged(nameof(ActualTextColor));
                }
            }
        }

        public string ActualText
        {
            get { return Quantity > 0 ? "В наличии" : "Нет в наличии"; }
        }

        public string ActualTextColor
        {
            get { return Quantity > 0 ? "Green" : "Red"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MedicineQuantitiy()
        {
            Medicine = new HashSet<Medicine>();
        }

        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}

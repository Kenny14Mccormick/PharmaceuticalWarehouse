//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Аптечный_склад
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Medicine:INotifyPropertyChanged
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicine()
        {
            this.ApplicationContent = new HashSet<ApplicationContent>();
            this.PharmacySupplyContent = new HashSet<PharmacySupplyContent>();
            this.SupplyContent = new HashSet<SupplyContent>();
            Quantity = 1; // Инициализация свойства Quantity
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        private bool _isAdded;
        public bool IsAdded
        {
            get { return _isAdded; }
            set
            {
                if (_isAdded != value)
                {
                    _isAdded = value;
                    OnPropertyChanged(nameof(IsAdded));
                    OnPropertyChanged(nameof(ButtonText)); // Уведомляем об изменении текста кнопки

                }
            }
        }
        public string ButtonText => IsAdded ? "Добавлено" : "Добавить";

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int MedicineCode { get; set; }
        public string Title { get; set; }
        public string Registration_number { get; set; }
        public int CategoryCode { get; set; }
        public int SubstanceCode { get; set; }
        public int FormCode { get; set; }
        public int PhotoCode { get; set; }
        public int ManufacturerCode { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string StorageConditionals { get; set; }
        public int QuantityCode { get; set; }
        public int PriceCode { get; set; }
    
        public virtual ActiveSubstance ActiveSubstance { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationContent> ApplicationContent { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual MedicineCategory MedicineCategory { get; set; }
        public virtual MedicinePhoto MedicinePhoto { get; set; }
        public virtual MedicinePrice MedicinePrice { get; set; }
        public virtual MedicineQuantitiy MedicineQuantitiy { get; set; }
        public virtual ReleaseForm ReleaseForm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacySupplyContent> PharmacySupplyContent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyContent> SupplyContent { get; set; }
    }
}

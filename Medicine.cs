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
    
    public partial class Medicine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicine()
        {
            this.ApplicationContent = new HashSet<ApplicationContent>();
            this.SupplyContent = new HashSet<SupplyContent>();
            this.WareHouseMedicine = new HashSet<WareHouseMedicine>();
        }
    
        public int MedicineCode { get; set; }
        public string Title { get; set; }
        public string Registration_number { get; set; }
        public int CategoryCode { get; set; }
        public int SubstanceCode { get; set; }
        public int FormCode { get; set; }
        public int PhotoCode { get; set; }
        public int ManufacturerCode { get; set; }
        public double Price { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string StorageConditionals { get; set; }
    
        public virtual ActiveSubstance ActiveSubstance { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationContent> ApplicationContent { get; set; }
        public virtual MedicineCategory MedicineCategory { get; set; }
        public virtual MedicinePhoto MedicinePhoto { get; set; }
        public virtual ReleaseForm ReleaseForm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyContent> SupplyContent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WareHouseMedicine> WareHouseMedicine { get; set; }
    }
}

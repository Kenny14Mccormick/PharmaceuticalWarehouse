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
    
    public partial class MedicineSupply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicineSupply()
        {
            this.SupplyContent = new HashSet<SupplyContent>();
        }
    
        public int SupplyCode { get; set; }
        public System.DateTime Date { get; set; }
        public int SupplierCode { get; set; }
        public int PharmacyManagerCode { get; set; }
        public double TotalCost
        {
            get
            {
                double totalcost = 0;
                foreach (var content in SupplyContent)
                {
                    totalcost += content.MedicineTotalCost;
                }
                return totalcost;
            }
        }

        public virtual PharmacyManager PharmacyManager { get; set; }
        public virtual Supplier Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyContent> SupplyContent { get; set; }
    }
}

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
    
    public partial class PharmacySupply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PharmacySupply()
        {
            this.PharmacySupplyContent = new HashSet<PharmacySupplyContent>();
            this.Application = new HashSet<Application>();
        }
    
        public int SupplyCode { get; set; }
        public int DisplaySupplyCode { get; set; }

        public int PharmacyCode { get; set; }
        public System.DateTime Date { get; set; }
        public int PharmacyManagerCode { get; set; }
    
        public virtual Pharmacy Pharmacy { get; set; }
        public virtual PharmacyManager PharmacyManager { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacySupplyContent> PharmacySupplyContent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Application { get; set; }
    }
}

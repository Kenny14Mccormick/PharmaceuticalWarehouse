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
    
    public partial class Application
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Application()
        {
            this.ApplicationContent = new HashSet<ApplicationContent>();
            this.PharmacySupply = new HashSet<PharmacySupply>();
        }
    
        public int ApplicationCode { get; set; }
        public string DisplayApplicationCode { get; set; }
        public int PharmacyCode { get; set; }
        public System.DateTime Date { get; set; }
        public int StatusCode { get; set; }

        public double TotalCost { get; set; }
        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationContent> ApplicationContent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacySupply> PharmacySupply { get; set; }
    }
}

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
    
    public partial class MedicineQuantitiy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicineQuantitiy()
        {
            this.Medicine = new HashSet<Medicine>();
        }
    
        public int QuantityCode { get; set; }
        public int Quantity { get; set; }

        public string ActualText
        {
            get
            {
                if (Quantity > 0)
                    return "В наличии";
                else
                    return "Нет в наличии";
            }
        }

        public string ActualTextColor
        {
            get
            {
                if (Quantity > 0)
                    return "Green";
                else
                    return "Red";
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}

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
    
    public partial class WareHouseMedicine
    {
        public int WareHouseCode { get; set; }
        public int MedicineCode { get; set; }
        public int Quantity { get; set; }
    
        public virtual Medicine Medicine { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}
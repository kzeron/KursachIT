//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KursachIT.DataFolder
{
    using System;
    using System.Collections.Generic;
    
    public partial class PrinterDetails
    {
        public int IdPrinter { get; set; }
        public int IdPrintTechnology { get; set; }
        public string MaxResolution { get; set; }
        public double MaxPrintSpeed { get; set; }
        public Nullable<int> IdColorTech { get; set; }
        public int IdDevice { get; set; }
    
        public virtual ColorTechology ColorTechology { get; set; }
        public virtual Devices Devices { get; set; }
        public virtual PrintTechonogy PrintTechonogy { get; set; }
    }
}

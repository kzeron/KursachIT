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
    
    public partial class Devices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Devices()
        {
            this.PCDetails = new HashSet<PCDetails>();
            this.PrinterDetails = new HashSet<PrinterDetails>();
            this.ScannerDetails = new HashSet<ScannerDetails>();
            this.ServerDetails = new HashSet<ServerDetails>();
        }
    
        public int IdDevice { get; set; }
        public string NameDevice { get; set; }
        public string SerialNumber { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public System.DateTime WarrantyEndDate { get; set; }
        public Nullable<System.DateTime> DateOfReceipt { get; set; }
        public int IdDeviceType { get; set; }
        public int IdBrand { get; set; }
        public Nullable<int> IdEmployer { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual DeviceTypes DeviceTypes { get; set; }
        public virtual Employers Employers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PCDetails> PCDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrinterDetails> PrinterDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScannerDetails> ScannerDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServerDetails> ServerDetails { get; set; }
    }
}

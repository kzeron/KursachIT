using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace KursachIT.ClassFolder
{
    internal class ClassDevice
    {
        public int IdDevice {  get; set; }
        public string DeviceName { get; set; }
        public string DeviceTypeName { get; set; }
        public int DeviceTypeId { get; set; }
        public string SerialNumber {  get; set; }
        public DateTimeValueSerializer PurchaseDate { get; set; }
        public DateTimeValueSerializer WarrantyEndDate { get; set; }
        public string CPU {  get; set; }

    }
}

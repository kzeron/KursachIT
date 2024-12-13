using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    internal class ClassDataValidator
    {
        public static bool IsPurchaseBeforeWarranty(DateTime purchaseDate, DateTime warrantyEndDate)
        {
            return purchaseDate <= warrantyEndDate;
        }
        public static bool IsReceiptInRange(DateTime receiptDate, DateTime purchaseDate, DateTime warrantyEndDate)
        {
            return receiptDate >= purchaseDate && receiptDate <= warrantyEndDate;
        }
    }
}

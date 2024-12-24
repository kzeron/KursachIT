using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            string pattern = @"^\+?[0-9]+$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        // Проверка: Валидность email (допустимые домены)
        public static bool IsEmailValid(string email)
        {
            string pattern = @"^[^@\s]+@(?:gmail\.com|yandex\.ru|mail\.ru)$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        // Проверка: Соотношение разрешения (формат ширина*высота)
        public static bool IsResolutionValid(string resolution)
        {
            string pattern = @"^\d+\*\d+$"; // Формат: цифры*цифры
            if (Regex.IsMatch(resolution, pattern))
            {
                var dimensions = resolution.Split('*');
                if (int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
                {
                    return width > 0 && height > 0;
                }
            }
            return false;
        }
    }
}

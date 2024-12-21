using KursachIT.PageFolder.AddPages.AddDeviceMore;
using KursachIT.PageFolder.EditPages.EditDeviceMore;
using System.Windows.Controls;

namespace KursachIT.ClassFolder
{
    internal static class DevicePageSelectorcs
    {
            public static Page GetPageForDeviceType(int idDeviceType, int deviceId)
            {
                switch (idDeviceType)
                {
                    case 1: // Тип устройства: ПК
                        return new AddPC(deviceId);
                    case 2: // Тип устройства: Сервер
                        return new AddPrinter(deviceId);
                    case 3: // Тип устройства: Сканер
                        return new AddScanner(deviceId);
                    case 4: // Тип устройства: Принтер
                        return new AddServer(deviceId);
                    default:
                        return null; // Неизвестный тип устройства
                }
            }
        public static Page GetEditPageForDeviceType(int idDeviceType, int deviceId)
        {
            switch (idDeviceType)
            {
                case 1: // Тип устройства: ПК
                    return new EditPC(deviceId);
                case 2: // Тип устройства: Принтер
                    return new EditPrinter(deviceId);
                case 3: // Тип устройства: Сканер
                    return new EditScanner(deviceId);
                case 4: // Тип устройства: Сервер
                    return new EditServer(deviceId);
                default:
                    return null; // Неизвестный тип устройства
            }
        }
    }
}

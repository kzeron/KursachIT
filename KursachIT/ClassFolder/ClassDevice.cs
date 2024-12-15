using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace KursachIT.ClassFolder
{
    public static class ShareDeviceData
    {
        public static int IdDeviceShare { get; set; }
    }
    internal class ClassDevice
    {
        public int IdDevice {  get; set; }
        public string NameDevice { get; set; }
        public string DeviceTypeName { get; set; }
        public int DeviceTypeId { get; set; }
        public string SerialNumber {  get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public string CPU {  get; set; }
        public int RAM { get; set; }
        public string Storage {  get; set; }
        public string GPU { get; set; }
        public int IdPrinter { get; set; }
        public string PrintTechnology { get; set; }
        public string MaxResolution { get; set; }
        public float MaxPrintSpeed { get; set; }
        public string ColorPrinting { get; set; }
        public int IdScanner { get; set; } 
        public string MaxScanResolution { get; set; }
        public string ScanSpeed { get; set; }
        public string DocumentFeeder { get; set; }
        public int RackUnit { get; set; }
        public string NetworkInterface { get; set; }
        public int? IdEmployer { get; set; }
        public int IdPrintTechnology {  get; set; }
        public int IdColorTech {  get; set; }
    }
    public class DeviceTypeModel
    {
        public string Name { get; set; }
        public DeviceType Type { get; set; }
    }
    public enum DeviceType
    {
        PC,
        Server,
        Scanner,
        Printer
    }
}

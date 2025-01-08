using KursachIT.ClassFolder;
using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursachIT.PageFolder.EditPages.EditDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для EditPC.xaml
    /// </summary>
    public partial class EditPC : Page
    {
        private int _idDevice;
        private PCDetails _currentPC;
        public EditPC(int idDevice)
        {
            InitializeComponent();
            _idDevice = idDevice;
            LoadData();
        }
        private void LoadData()
        {
            using (var context = new ITAdminEntities())
            {
                _currentPC = context.PCDetails.FirstOrDefault(pc => pc.IdDevice == _idDevice);

                if (_currentPC != null)
                {
                    CPUTb.Text = _currentPC.CPU;
                    RAMTb.Text = _currentPC.RAM.ToString();
                    StorageTb.Text = _currentPC.Storage;
                    GPUTb.Text = _currentPC.GPU;
                }
            }
        }
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CPUTb.Text))
            {
                MBClass.ErrorMB("Введите CPU");
                return;
            }
            else if (string.IsNullOrWhiteSpace(RAMTb.Text))
            {
                MBClass.ErrorMB("Введите RAM");
                return;
            }
            else if (string.IsNullOrWhiteSpace(StorageTb.Text))
            {
                MBClass.ErrorMB("Введите Storage");
                return;
            }
            else if (string.IsNullOrWhiteSpace(GPUTb.Text)) // Assuming a typo here, using the existing RAMTb for GPU
            {
                MBClass.ErrorMB("Введите GPU");
                return;
            }

            try
            {
                using (var context = new ITAdminEntities())
                {
                    int ramSize;
                    if (!int.TryParse(RAMTb.Text, out ramSize))
                    {
                        MBClass.ErrorMB("Ошибка ввода RAM");
                        return;
                    }

                    if (_currentPC == null)
                    {
                        _currentPC = new PCDetails();
                    }

                    _currentPC.CPU = CPUTb.Text;
                    _currentPC.RAM = ramSize;
                    _currentPC.Storage = StorageTb.Text;
                    _currentPC.GPU = GPUTb.Text; // Assuming a typo here, using the existing RAMTb for GPU
                    _currentPC.IdDevice = _idDevice;

                    context.PCDetails.Attach(_currentPC);
                    context.Entry(_currentPC).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                Window.GetWindow(this).Close();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex.Message);
            }
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

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

namespace KursachIT.PageFolder.AddPages.AddDeviceMore
{
    /// <summary>
    /// Логика взаимодействия для AddPC.xaml
    /// </summary>
    public partial class AddPC : Page
    {
        private int IdDevice;
        public AddPC(int idDivece)
        {
            IdDevice = idDivece;
            InitializeComponent();
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CPUTb.Text))
            {
                MBClass.ErrorMB("Введите имя");
            }
            else if (string.IsNullOrWhiteSpace(RAMTb.Text))
            {
                MBClass.ErrorMB("Введите Фамилию");
            }
            else if (string.IsNullOrWhiteSpace(StorageTb.Text))
            {
                MBClass.ErrorMB("Введите почту");
            }
            else if (string.IsNullOrWhiteSpace(RAMTb.Text))
            {
                MBClass.ErrorMB("Укажите телефон");
            }
            else
            {
                try
                {
                    using (var context = new ITAdminEntities())
                    {
                        int ramSize;
                        if(!int.TryParse(RAMTb.Text, out ramSize))
                        {
                            return;
                        }
                        var pc = new PCDetails
                        {
                            CPU = CPUTb.Text,
                            RAM = ramSize,
                            Storage = StorageTb.Text,
                            GPU = RAMTb.Text,
                            IdDevice = IdDevice
                        };
                        context.PCDetails.Add(pc);
                        context.SaveChanges();
                    }
                    Window.GetWindow(this).Close();
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }
        }
    }
}

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

namespace KursachIT.PageFolder.MoreInfoFolder
{
    /// <summary>
<<<<<<<< HEAD:KursachIT/PageFolder/MoreInfoFolder/MorePrinter.xaml.cs
    /// Логика взаимодействия для MorePrinter.xaml
    /// </summary>
    public partial class MorePrinter : Page
    {
        public MorePrinter()
========
    /// Логика взаимодействия для MorePC.xaml
    /// </summary>
    public partial class MorePC : Page
    {
        public MorePC()
>>>>>>>> ea3e41d804a44bf6a5b8440be9579bc25df97f4f:KursachIT/PageFolder/MoreInfoFolder/MorePC.xaml.cs
        {
            InitializeComponent();
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

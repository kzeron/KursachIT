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

namespace KursachIT.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для PersAcc.xaml
    /// </summary>
    public partial class PersAcc : Page
    {
        public PersAcc()
        {
            InitializeComponent();
            var currentUser = ClassSaveSassion.LoadSession();

            if (currentUser != null)
            {
                // Получаем данные сотрудника, связанного с текущим пользователем
                var employee = ITAdminEntities.GetContext().Employers
                    .FirstOrDefault(emp => emp.IdUser == currentUser.IdLogin);

                if (employee != null)
                {
                    DataContext = employee;
                }
                else
                {
                    MBClass.ErrorMB("Информация о сотруднике не найдена.");
                }
            }
            else
            {
                MBClass.ErrorMB("Сессия пользователя не найдена. Проверьте вход в систему.");
            }
        }
    }
}

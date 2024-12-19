﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KursachIT.ClassFolder;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursachIT.PageFolder.MoreFolder
{
    /// <summary>
    /// Логика взаимодействия для MoreEployer.xaml
    /// </summary>
    public partial class MoreEployer : Page
    {
        public MoreEployer(ClassUser selectedUser)
        {
            InitializeComponent();
            DataContext = selectedUser;
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

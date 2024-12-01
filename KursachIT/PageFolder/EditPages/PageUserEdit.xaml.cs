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

namespace KursachIT.PageFolder.EditPages
{
    /// <summary>
    /// Логика взаимодействия для PageUserEdit.xaml
    /// </summary>
    public partial class PageUserEdit : Page
    {
        private ClassUser _user;

        public PageUserEdit(int classUser)
        {
            InitializeComponent();
            _user.IdUser = classUser;
        }
    }
}

﻿using SchoolSessionWPF.ADOApp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolSessionWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SessionOneEntities Connection = new SessionOneEntities();
    }
}

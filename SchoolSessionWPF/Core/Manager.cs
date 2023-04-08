using SchoolSessionWPF.ADOApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SchoolSessionWPF.Core
{
    class Manager
    {
        public static Frame MainFrame { get; set; }
        public static bool IsAdminMode = false;
        public static int AllRecordsCount { get; set; }
        public static int FindRecordsCount { get; set; }
    }
}

using SchoolSessionWPF.ADOApp;
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

namespace SchoolSessionWPF.PagesApp
{
    /// <summary>
    /// Interaction logic for ClientServiceAddPage.xaml
    /// </summary>
    public partial class ClientServiceAddPage : Page
    {
        private Service _service = new Service();
        private TimeSpan _startTime;

        public ClientServiceAddPage(Service service)
        {
            InitializeComponent();

            _service = service;
            DataContext = _service;
        }

        private void tbTimeStart_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

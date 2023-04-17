using SchoolSessionWPF.ADOApp;
using SchoolSessionWPF.Core;
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
    /// Interaction logic for ServiceAddEditPage.xaml
    /// </summary>
    public partial class ServiceAddEditPage : Page
    {
        private static Service _currentService = new Service();
        private bool _isEdit;

        public ServiceAddEditPage(Service selectedService)
        {
            InitializeComponent();

            if (selectedService != null)
                _isEdit = true;
            else selectedService = new Service();

            SetIdState();

            _currentService = selectedService;
            DataContext = _currentService;
        }

        public static Service GetCurrentService()
        {
            return _currentService;
        }

        private void SetIdState()
        {
            if (_isEdit)
            {
                GridID.Visibility = Visibility.Visible;
                GridID.IsEnabled = false;
            }
            else
            {
                GridID.Visibility = Visibility.Collapsed;
                GridID.IsEnabled = true;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_isEdit)
            {
                SessionOneEntities.GetContext().Entry(_currentService).Reload();
            }
        }
    }
}

using SchoolSessionWPF.ADOApp;
using SchoolSessionWPF.Core;
using SchoolSessionWPF.PagesApp;
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

namespace SchoolSessionWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ServicesPage());
            Manager.MainFrame = MainFrame;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (Manager.MainFrame.CanGoBack) BtnBack.Visibility = Visibility.Visible;
            else BtnBack.Visibility = Visibility.Collapsed;

            if (Manager.MainFrame.Content is ServiceAddEditPage) BtnSave.Visibility = Visibility.Visible;
            else BtnSave.Visibility = Visibility.Collapsed;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var service = ServiceAddEditPage.GetCurrentService();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(service.Title))
                errors.AppendLine("Введите название услуги");
            if (service.Cost == null)
                errors.AppendLine("Укажите стоимость");
            if (service.DurationInSeconds == null)
                errors.AppendLine("Укажите длительность услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (service.ID == 0)
                SessionOneEntities.GetContext().Service.Add(service);

            try
            {
                SessionOneEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Visibility = Visibility.Visible;

            if (Visibility == Visibility.Visible)
            {
                tbRecordsCount.Text = $"{Manager.FindRecordsCount} из {Manager.AllRecordsCount}";
            }
        }
    }
}

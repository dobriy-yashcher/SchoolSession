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
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private List<Service> _services;
        private List<Service> _sorted;

        private Predicate<Service> _filterQuery = x => true;
        private Func<Service, object> _sortQuery = x => x.ID;

        public ServicesPage()
        {
            InitializeComponent();
            PageLoaded();
        }

        private void PageLoaded()
        {
            _services = SessionOneEntities.GetContext().Service.ToList();
            lvServices.ItemsSource = _services;

            cbSorting.ItemsSource = SortingMethods.Methods;
            cbFiltering.ItemsSource = FilterMethods.Methods;
            cbSorting.SelectedIndex = 0;
            cbFiltering.SelectedIndex = 0;
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortingMethodChange();
        }

        private void cbFiltering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMethodChange();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void FilterAndSort()
        {                        
            _sorted = _services.Where(x => _filterQuery(x)).OrderBy(x => _sortQuery(x)).ToList();
            lvServices.ItemsSource = _sorted;

            if (tbSearchBar.Text != "") Search();

            UpdateRecordsCount();
        }

        private void Search()
        {
            if (tbSearchBar.Text == "0000") Manager.IsAdminMode = !Manager.IsAdminMode;

            lvServices.ItemsSource = _sorted
                .Where(x => string.Join(" ", x.Title, x.Description).ToLower()
                .Contains(tbSearchBar.Text.ToLower()))
                .ToList();
            UpdateRecordsCount();

            Manager.MainFrame.Refresh();
        }

        private void SortingMethodChange()
        {
            switch (cbSorting.SelectedIndex)
            {
                case 0:
                    _sortQuery = x => x.ID;
                    break;
                case 1:
                    _sortQuery = x => x.CostWithDiscount;
                    break;
                case 2:
                    _sortQuery = x => -x.CostWithDiscount;
                    break;
            }

            FilterAndSort();
        }

        private void FilterMethodChange()
        {
            switch (cbFiltering.SelectedIndex)
            {
                case 0:
                    _filterQuery = x => true;
                    break;
                case 1:
                    _filterQuery = x => x.Discount >= 0 && x.Discount < 5;
                    break;
                case 2:
                    _filterQuery = x => x.Discount >= 5 && x.Discount < 15;
                    break;
                case 3:
                    _filterQuery = x => x.Discount >= 15 && x.Discount < 30;
                    break;
                case 4:
                    _filterQuery = x => x.Discount >= 30 && x.Discount < 70;
                    break;
                case 5:
                    _filterQuery = x => x.Discount >= 70 && x.Discount < 100;
                    break;
                default:
                    _filterQuery = x => true;
                    break;
            }
            FilterAndSort();
        }

        private void UpdateRecordsCount()
        {
            Manager.AllRecordsCount = _services.Count();
            Manager.FindRecordsCount = lvServices.Items.Count;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var serviceRemoving = (sender as Button).DataContext as Service;

            if (MessageBox.Show("Вы точно хотите удалить данную услугу?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    SessionOneEntities.GetContext().Service.Remove(serviceRemoving);
                    SessionOneEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    UpdateSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }               
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServiceAddEditPage((sender as Button).DataContext as Service));
        }

        private void UpdateSource()
        {
            _services = SessionOneEntities.GetContext().Service.ToList();
            lvServices.ItemsSource = _services;
            FilterAndSort();
        }
    }
}

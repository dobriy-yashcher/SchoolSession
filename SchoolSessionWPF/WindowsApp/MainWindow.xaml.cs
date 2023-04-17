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

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            SetStateNavButtons();

            tbRecordsCount.Text = $"{Manager.FindRecordsCount} из {Manager.AllRecordsCount}";
        }

        private void SetStateNavButtons()
        {
            if (Manager.MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
                tbRecordsCount.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnBack.Visibility = Visibility.Collapsed;
                tbRecordsCount.Visibility = Visibility.Visible;
            }

            if (Manager.MainFrame.Content is ServiceAddEditPage) BtnSave.Visibility = Visibility.Visible;
            else BtnSave.Visibility = Visibility.Collapsed;

            if (!Manager.IsAdminMode || Manager.MainFrame.CanGoBack) BtnAddService.Visibility = Visibility.Collapsed;
            else BtnAddService.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var maxDurationTimeInSeconds = 60 * 60 * 4;

            var service = ServiceAddEditPage.GetCurrentService();
            StringBuilder errors = new StringBuilder();

            bool isAddService = service.ID == 0 ? true : false;

            // Проверка данных
            if (string.IsNullOrWhiteSpace(service.Title))
                errors.AppendLine("Введите название услуги");
            if (service.Cost <= 0)
                errors.AppendLine("Укажите корректную стоимость");
            if (service.DurationInSeconds <= 0)
                errors.AppendLine("Укажите длительность услуги");    
            if (service.DurationInSeconds > maxDurationTimeInSeconds)
                errors.AppendLine($"Длительность услуги не может превышать {maxDurationTimeInSeconds / 60 / 60} часа");
            if (service.Discount < 0)
                errors.AppendLine("Скидка не может принимать отрицательное значение");
            if (service.Discount > 100)
                errors.AppendLine("Скидка не может принимать значение больше 100");

            // Проверка (при добавление новой услуги), существует ли услуга с таким названием
            if (isAddService)
            {                                                                                             
                var checkTitle = SessionOneEntities.GetContext().Service
                    .Where(x => x.Title.ToLower()
                    .Contains(service.Title.ToLower()))
                    .ToList();

                if (checkTitle.Count > 0)
                    errors.AppendLine("Услуга с таким названием уже существует");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Определение текущей операции (добавление/редактирование)
            if (isAddService)
                SessionOneEntities.GetContext().Service.Add(service);

            try
            {
                SessionOneEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServiceAddEditPage(null));
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Praktika3.Data;

namespace Praktika3
{
    public partial class RequestsView : Page
    {
        private AppDbContext _db;
        private Request _selectedRequest;

        public RequestsView()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests(string status = null)
        {
            if (RequestsGrid == null) return;
            _db = new AppDbContext();
            var query = _db.Requests
                .Include(r => r.Partner)
                .Include(r => r.Items)
                .OrderByDescending(r => r.CreatedDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "Все")
            {
                int code = GetStatusCode(status);
                query = query.Where(r => (int)r.Status == code);
            }
            var items = query.ToList();
            RequestsGrid.ItemsSource = items;
            if (_selectedRequest != null)
            {
                var newSelected = items.FirstOrDefault(r => r.Id == _selectedRequest.Id);
                if (newSelected != null)
                    RequestsGrid.SelectedItem = newSelected;
                else
                    _selectedRequest = null;
            }
        }

        private int GetStatusCode(string name)
        {
            switch (name)
            {
                case "Создана": return 0;
                case "Предоплата": return 1;
                case "Производство": return 2;
                case "Доставка": return 3;
                case "Выполнена": return 4;
                case "Отменена": return 5;
                default: return -1;
            }
        }

        private void UpdateButtons(Request r)
        {
            if (r == null)
            {
                BtnPrepayment.Visibility = BtnProduction.Visibility = BtnDelivery.Visibility =
                BtnFullPayment.Visibility = BtnComplete.Visibility = BtnCancel.Visibility = Visibility.Collapsed;
                return;
            }
            BtnPrepayment.Visibility = r.Status == RequestStatus.Created ? Visibility.Visible : Visibility.Collapsed;
            BtnProduction.Visibility = r.Status == RequestStatus.Prepayment ? Visibility.Visible : Visibility.Collapsed;
            BtnDelivery.Visibility = r.Status == RequestStatus.Production ? Visibility.Visible : Visibility.Collapsed;
            BtnFullPayment.Visibility = r.Status == RequestStatus.Delivery ? Visibility.Visible : Visibility.Collapsed;
            BtnComplete.Visibility = r.Status == RequestStatus.Delivery ? Visibility.Visible : Visibility.Collapsed;
            BtnCancel.Visibility = r.Status == RequestStatus.Created ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadItems(int requestId)
        {
            var items = _db.RequestItems.Include(i => i.Product).Where(i => i.RequestId == requestId).ToList();
            ItemsGrid.ItemsSource = items;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadRequests();

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var filter = ((ComboBoxItem)StatusFilter.SelectedItem)?.Content.ToString();
            LoadRequests(filter);
        }

        private void RequestsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRequest = (Request)RequestsGrid.SelectedItem;
            if (_selectedRequest != null)
            {
                LoadItems(_selectedRequest.Id);
                UpdateButtons(_selectedRequest);
            }
            else
            {
                ItemsGrid.ItemsSource = null;
                UpdateButtons(null);
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateRequestDialog(new AppDbContext());
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadRequests();
            }
        }

        private void BtnPrepayment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            _selectedRequest.Status = RequestStatus.Prepayment;
            _selectedRequest.PrepaymentDate = DateTime.Now;
            _db.SaveChanges();
            LoadRequests();
            MessageBox.Show($"Предоплата по заявке №{id} внесена");
        }

        private void BtnProduction_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            _selectedRequest.Status = RequestStatus.Production;
            _selectedRequest.ProductionDate = DateTime.Now;
            _db.SaveChanges();
            LoadRequests();
            MessageBox.Show($"Заявка №{id} запущена в производство");
        }

        private void BtnDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            _selectedRequest.Status = RequestStatus.Delivery;
            _db.SaveChanges();
            LoadRequests();
            MessageBox.Show($"Предложите партнеру организовать доставку заявки №{id}");
        }

        private void BtnFullPayment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            var total = _db.RequestItems.Where(ri => ri.RequestId == id).Sum(ri => ri.Quantity * ri.Price);
            var remaining = total - _selectedRequest.Prepayment;
            if (MessageBox.Show($"Полная оплата: {total:N2} ₽\nПредоплата: {_selectedRequest.Prepayment:N2} ₽\nОсталось доплатить: {remaining:N2} ₽\n\nПодтверждаете получение и оплату?",
                "Полная оплата", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _selectedRequest.Status = RequestStatus.Completed;
                _selectedRequest.CompletionDate = DateTime.Now;
                _db.SaveChanges();
                var partner = _db.Partners.Find(_selectedRequest.PartnerId);
                if (partner != null)
                    partner.TotalSales += total;
                _db.SaveChanges();
                LoadRequests();
                MessageBox.Show($"Заявка №{id} выполнена. Полная оплата получена.");
            }
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            _selectedRequest.Status = RequestStatus.Completed;
            _selectedRequest.CompletionDate = DateTime.Now;
            _db.SaveChanges();
            LoadRequests();
            MessageBox.Show($"Заявка №{id} выполнена.");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRequest == null)
            {
                MessageBox.Show("Выберите заявку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = _selectedRequest.Id;
            if (MessageBox.Show($"Отменить заявку №{id}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _selectedRequest.Status = RequestStatus.Cancelled;
                _db.SaveChanges();
                LoadRequests();
                MessageBox.Show($"Заявка №{id} отменена");
            }
        }
    }
}
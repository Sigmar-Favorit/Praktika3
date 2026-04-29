using System;
using System.Linq;
using System.Windows;
using Praktika3.Data;

namespace Praktika3
{
    public partial class CreateRequestDialog : Window
    {
        private readonly AppDbContext _context;
        public Request CreatedRequest { get; private set; }

        public CreateRequestDialog(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadData();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void LoadData()
        {
            cbPartner.ItemsSource = _context.Partners.ToList();
            cbProduct.ItemsSource = _context.Products.ToList();
            dpDate.SelectedDate = DateTime.Now.AddDays(7);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbPartner.SelectedItem == null || cbProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите партнера и продукцию");
                return;
            }

            if (!int.TryParse(tbQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            var partner = (Partner)cbPartner.SelectedItem;
            var product = (Product)cbProduct.SelectedItem;
            decimal total = product.MinPrice * qty;
            decimal prepayment = total * 0.3m;

            CreatedRequest = new Request
            {
                PartnerId = partner.Id,
                CreatedDate = DateTime.Now,
                ProductionDate = dpDate.SelectedDate,
                Prepayment = prepayment,
                Status = RequestStatus.Created
            };

            _context.Requests.Add(CreatedRequest);
            _context.SaveChanges();

            _context.RequestItems.Add(new RequestItem
            {
                RequestId = CreatedRequest.Id,
                ProductId = product.Id,
                Quantity = qty,
                Price = product.MinPrice,
                ProductionDate = dpDate.SelectedDate
            });

            _context.SaveChanges();
            MessageBox.Show($"Заявка №{CreatedRequest.Id} создана! Предоплата: {prepayment:N2} ₽");
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
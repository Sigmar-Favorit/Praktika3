using System;
using System.Linq;
using System.Windows;
using Praktika3.Data;

namespace Praktika3
{
    public partial class AddProductDialog : Window
    {
        public Product NewProduct { get; private set; }

        public AddProductDialog()
        {
            InitializeComponent();
            LoadProductTypes();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += (s, e) => DialogResult = false;
        }

        private void LoadProductTypes()
        {
            using (var db = new AppDbContext())
            {
                cbProductType.ItemsSource = db.ProductTypes.ToList();
                if (cbProductType.Items.Count > 0)
                    cbProductType.SelectedIndex = 0;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbArticle.Text))
            {
                MessageBox.Show("Введите артикул");
                return;
            }
            if (cbProductType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип продукции");
                return;
            }
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Введите наименование");
                return;
            }
            if (!decimal.TryParse(tbMinPrice.Text, out decimal minPrice))
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }
            if (!int.TryParse(tbProductionTime.Text, out int prodTime))
            {
                MessageBox.Show("Введите корректное время изготовления");
                return;
            }
            if (!int.TryParse(tbWorkshop.Text, out int workshop))
            {
                MessageBox.Show("Введите корректный номер цеха");
                return;
            }
            if (!decimal.TryParse(tbCostPrice.Text, out decimal costPrice))
            {
                MessageBox.Show("Введите корректную себестоимость");
                return;
            }

            var productType = (ProductType)cbProductType.SelectedItem;

            NewProduct = new Product
            {
                Article = tbArticle.Text,
                Type = productType.TypeName,
                Name = tbName.Text,
                MinPrice = minPrice,
                ProductionTimeHours = prodTime,
                WorkshopNumber = workshop,
                CostPrice = costPrice,
                ProductTypeId = productType.Id,
                EmployeesCount = 1,
                Description = "",
                StandardNumber = ""
            };

            DialogResult = true;
            Close();
        }
    }
}
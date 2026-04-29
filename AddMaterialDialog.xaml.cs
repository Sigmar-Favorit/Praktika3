using System;
using System.Linq;
using System.Windows;
using Praktika3.Data;

namespace Praktika3
{
    public partial class AddMaterialDialog : Window
    {
        public Material NewMaterial { get; private set; }

        public AddMaterialDialog()
        {
            InitializeComponent();
            LoadMaterialTypes();
            LoadSuppliers();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += (s, e) => DialogResult = false;
        }

        private void LoadMaterialTypes()
        {
            using (var db = new AppDbContext())
            {
                cbMaterialType.ItemsSource = db.MaterialTypes.ToList();
                if (cbMaterialType.Items.Count > 0)
                    cbMaterialType.SelectedIndex = 0;
            }
        }

        private void LoadSuppliers()
        {
            using (var db = new AppDbContext())
            {
                cbSupplier.ItemsSource = db.Suppliers.ToList();
                if (cbSupplier.Items.Count > 0)
                    cbSupplier.SelectedIndex = 0;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbMaterialType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип материала");
                return;
            }
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Введите наименование");
                return;
            }
            if (cbSupplier.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика");
                return;
            }
            if (!int.TryParse(tbQtyPerPackage.Text, out int qtyPackage))
            {
                MessageBox.Show("Введите количество в упаковке");
                return;
            }
            if (string.IsNullOrWhiteSpace(tbUnit.Text))
            {
                MessageBox.Show("Введите единицу измерения");
                return;
            }
            if (!decimal.TryParse(tbCost.Text, out decimal cost))
            {
                MessageBox.Show("Введите стоимость");
                return;
            }
            if (!double.TryParse(tbQuantityStock.Text, out double stock))
            {
                MessageBox.Show("Введите начальное количество на складе");
                return;
            }
            if (!double.TryParse(tbMinQuantity.Text, out double minQty))
            {
                MessageBox.Show("Введите минимальное количество");
                return;
            }

            var materialType = (MaterialType)cbMaterialType.SelectedItem;
            var supplier = (Supplier)cbSupplier.SelectedItem;

            NewMaterial = new Material
            {
                Type = materialType.TypeName,
                Name = tbName.Text,
                SupplierId = supplier.Id,
                QuantityPerPackage = qtyPackage,
                UnitOfMeasure = tbUnit.Text,
                Cost = cost,
                QuantityInStock = stock,
                MinQuantity = minQty,
                MaterialTypeId = materialType.Id,
                Description = ""
            };

            DialogResult = true;
            Close();
        }
    }
}
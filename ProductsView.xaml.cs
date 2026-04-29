using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Praktika3.Data;

namespace Praktika3
{
    public partial class ProductsView : Page
    {
        private AppDbContext _db;

        public ProductsView()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var db = new AppDbContext())
            {
                ProductsGrid.ItemsSource = db.Products.ToList();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadProducts();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddProductDialog();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Products.Add(dialog.NewProduct);
                    db.SaveChanges();
                    LoadProducts();
                    MessageBox.Show("Продукция добавлена");
                }
            }
        }
    }
}
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Praktika3.Data;

namespace Praktika3
{
    public partial class MaterialsView : Page
    {
        public MaterialsView()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            using (var db = new AppDbContext())
            {
                MaterialsGrid.ItemsSource = db.Materials.Include(m => m.Supplier).ToList();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadMaterials();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddMaterialDialog();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Materials.Add(dialog.NewMaterial);
                    db.SaveChanges();
                    LoadMaterials();
                    MessageBox.Show("Материал добавлен");
                }
            }
        }
    }
}
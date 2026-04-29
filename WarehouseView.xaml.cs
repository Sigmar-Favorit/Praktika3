using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Praktika3.Data;

namespace Praktika3
{
    public partial class WarehouseView : Page
    {
        private AppDbContext _db;

        public WarehouseView()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            _db = new AppDbContext();
            MaterialsGrid.ItemsSource = _db.Materials.ToList();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadMaterials();

        private void BtnReceive_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите материал");
                return;
            }

            var material = (Material)MaterialsGrid.SelectedItem;
            var dialog = new QuantityDialog("Поступление", material.Name);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                var oldQty = material.QuantityInStock;
                material.QuantityInStock += dialog.Quantity;

                _db.MaterialHistories.Add(new MaterialHistory
                {
                    MaterialId = material.Id,
                    OldQuantity = oldQty,
                    NewQuantity = material.QuantityInStock,
                    ChangeDate = DateTime.Now,
                    OperationType = "Поступление"
                });

                _db.WarehouseOperations.Add(new WarehouseOperation
                {
                    OperationDate = DateTime.Now,
                    OperationType = "Поступление",
                    MaterialId = material.Id,
                    Quantity = dialog.Quantity
                });

                _db.SaveChanges();
                LoadMaterials();
                MessageBox.Show($"Поступление {dialog.Quantity} {material.UnitOfMeasure}");
            }
        }

        private void BtnWriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите материал");
                return;
            }

            var material = (Material)MaterialsGrid.SelectedItem;
            var dialog = new QuantityDialog("Списание", material.Name);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                if (material.QuantityInStock - dialog.Quantity < 0)
                {
                    MessageBox.Show("Недостаточно материала на складе!");
                    return;
                }

                var oldQty = material.QuantityInStock;
                material.QuantityInStock -= dialog.Quantity;

                _db.MaterialHistories.Add(new MaterialHistory
                {
                    MaterialId = material.Id,
                    OldQuantity = oldQty,
                    NewQuantity = material.QuantityInStock,
                    ChangeDate = DateTime.Now,
                    OperationType = "Списание"
                });

                _db.WarehouseOperations.Add(new WarehouseOperation
                {
                    OperationDate = DateTime.Now,
                    OperationType = "Списание",
                    MaterialId = material.Id,
                    Quantity = dialog.Quantity
                });

                _db.SaveChanges();
                LoadMaterials();
                MessageBox.Show($"Списано {dialog.Quantity} {material.UnitOfMeasure}");
            }
        }
    }
}
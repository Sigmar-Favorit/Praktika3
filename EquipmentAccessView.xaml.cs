using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Praktika3.Data;

namespace Praktika3
{
    public partial class EquipmentAccessView : Page
    {
        private AppDbContext _db;

        public EquipmentAccessView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _db = new AppDbContext();
            EquipmentGrid.ItemsSource = _db.EquipmentAccesses
                .Include(e => e.Employee)
                .ToList();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadData();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEquipmentAccessDialog();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                _db.EquipmentAccesses.Add(dialog.NewAccess);
                _db.SaveChanges();
                LoadData();
                MessageBox.Show("Доступ добавлен");
            }
        }
    }
}
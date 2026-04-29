
using System.Windows;
using System.Windows.Controls;
using Praktika3.Data;

namespace Praktika3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            db.AutoCancelExpiredRequests();
            MainFrame.Content = new PartnersView();
        }

        private void BtnPartners_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new PartnersView();
        private void BtnRequests_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new RequestsView();
        private void BtnProducts_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new ProductsView();
        private void BtnMaterials_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new MaterialsView();
        private void BtnEmployees_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new EmployeesView();
        private void BtnWarehouse_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new WarehouseView();
        private void BtnEquipment_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new EquipmentAccessView();
        private void BtnMovements_Click(object sender, RoutedEventArgs e) => MainFrame.Content = new MovementsView();
    }
}
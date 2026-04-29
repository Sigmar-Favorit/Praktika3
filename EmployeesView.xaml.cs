using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Praktika3.Data;

namespace Praktika3
{
    public partial class EmployeesView : Page
    {
        private AppDbContext _db;

        public EmployeesView()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            _db = new AppDbContext();
            EmployeesGrid.ItemsSource = _db.Employees.ToList();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadEmployees();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEmployeeDialog();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                _db.Employees.Add(dialog.NewEmployee);
                _db.SaveChanges();
                LoadEmployees();
                MessageBox.Show("Сотрудник успешно добавлен");
            }
        }
    }
}
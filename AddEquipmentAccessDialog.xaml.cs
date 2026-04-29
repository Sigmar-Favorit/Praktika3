using System;
using System.Linq;
using System.Windows;
using Praktika3.Data;

namespace Praktika3
{
    public partial class AddEquipmentAccessDialog : Window
    {
        public EquipmentAccess NewAccess { get; private set; }

        public AddEquipmentAccessDialog()
        {
            InitializeComponent();
            LoadEmployees();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void LoadEmployees()
        {
            using (var db = new AppDbContext())
            {
                cbEmployee.ItemsSource = db.Employees.ToList();
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbEmployee.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника");
                return;
            }
            if (string.IsNullOrWhiteSpace(tbEquipment.Text))
            {
                MessageBox.Show("Введите название оборудования");
                return;
            }

            NewAccess = new EquipmentAccess
            {
                EmployeeId = (int)cbEmployee.SelectedValue,
                EquipmentName = tbEquipment.Text,
                MasterName = tbMaster.Text,
                GrantedDate = DateTime.Now
            };
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
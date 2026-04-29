using System;
using System.Windows;
using Praktika3.Data;

namespace Praktika3
{
    public partial class AddEmployeeDialog : Window
    {
        public Employee NewEmployee { get; private set; }

        public AddEmployeeDialog()
        {
            InitializeComponent();
            dpBirthDate.SelectedDate = DateTime.Now.AddYears(-30);
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFullName.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника");
                return;
            }

            NewEmployee = new Employee
            {
                FullName = tbFullName.Text,
                BirthDate = dpBirthDate.SelectedDate ?? DateTime.Now,
                PassportData = tbPassport.Text,
                BankDetails = tbBank.Text,
                HealthStatus = tbHealth.Text,
                HasFamily = cbHasFamily.IsChecked == true,
                AccessCardNumber = tbCard.Text
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
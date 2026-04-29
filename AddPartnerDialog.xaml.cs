using Praktika3.Data;
using System.Windows;
using System.Windows.Controls;

namespace Praktika3
{
    public partial class AddPartnerDialog : Window
    {
        public Partner NewPartner { get; private set; }

        public AddPartnerDialog()
        {
            InitializeComponent();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Введите наименование партнера");
                return;
            }

            if (!int.TryParse(tbRating.Text, out int rating) || rating < 1 || rating > 10)
            {
                MessageBox.Show("Рейтинг должен быть числом от 1 до 10");
                return;
            }

            NewPartner = new Partner
            {
                Type = (cbType.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                Name = tbName.Text,
                DirectorName = tbDirector.Text,
                Phone = tbPhone.Text,
                Email = tbEmail.Text,
                LegalAddress = tbAddress.Text,
                INN = tbINN.Text,
                Rating = rating,
                TotalSales = 0
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
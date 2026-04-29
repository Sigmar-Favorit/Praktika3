using System.Windows;

namespace Praktika3
{
    public partial class QuantityDialog : Window
    {
        public double Quantity { get; private set; }

        public QuantityDialog(string title, string materialName)
        {
            InitializeComponent();
            Title = title;
            tbMaterialName.Text = $"Материал: {materialName}";
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tbQuantity.Text, out double qty) || qty <= 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            Quantity = qty;
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
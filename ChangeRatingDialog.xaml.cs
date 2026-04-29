using System.Windows;

namespace Praktika3
{
    public partial class ChangeRatingDialog : Window
    {
        public int NewRating { get; private set; }

        public ChangeRatingDialog(int currentRating)
        {
            InitializeComponent();
            tbCurrentRating.Text = $"Текущий рейтинг: {currentRating}";
            tbNewRating.Text = currentRating.ToString();
            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbNewRating.Text, out int rating) || rating < 1 || rating > 10)
            {
                MessageBox.Show("Введите число от 1 до 10");
                return;
            }

            NewRating = rating;
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
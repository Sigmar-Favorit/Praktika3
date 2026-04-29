using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Praktika3.Data;

namespace Praktika3
{
    public partial class PartnersView : Page
    {
        private AppDbContext _db;

        public PartnersView()
        {
            InitializeComponent();
            LoadPartners();
        }

        private void LoadPartners()
        {
            _db = new AppDbContext();
            var partners = _db.Partners.ToList();
            foreach (var partner in partners)
            {
                partner.TotalSales = _db.RequestItems
                    .Where(ri => ri.Request.PartnerId == partner.Id && ri.Request.Status == RequestStatus.Completed)
                    .Sum(ri => ri.Quantity * ri.Price);
            }
            _db.SaveChanges();
            PartnersGrid.ItemsSource = partners.OrderByDescending(p => p.Rating).ToList();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadPartners();

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = SearchBox.Text.ToLower();
            if (string.IsNullOrEmpty(search))
                PartnersGrid.ItemsSource = _db.Partners.ToList();
            else
                PartnersGrid.ItemsSource = _db.Partners.Where(p => p.Name.ToLower().Contains(search)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddPartnerDialog();
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                _db.Partners.Add(dialog.NewPartner);
                _db.SaveChanges();
                LoadPartners();
                MessageBox.Show("Партнер успешно добавлен!");
            }
        }

        private void BtnChangeRating_Click(object sender, RoutedEventArgs e)
        {
            if (PartnersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите партнера");
                return;
            }

            var partner = (Partner)PartnersGrid.SelectedItem;
            var dialog = new ChangeRatingDialog(partner.Rating);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                var history = new RatingHistory
                {
                    PartnerId = partner.Id,
                    OldRating = partner.Rating,
                    NewRating = dialog.NewRating,
                    ChangeDate = DateTime.Now,
                    ChangedBy = "Менеджер"
                };
                partner.Rating = dialog.NewRating;
                _db.RatingHistories.Add(history);
                _db.SaveChanges();
                LoadPartners();
                MessageBox.Show($"Рейтинг изменен с {history.OldRating} на {history.NewRating}");
            }
        }
    }
}
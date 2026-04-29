using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Praktika3.Data;

namespace Praktika3
{
    public partial class MovementsView : Page
    {
        private AppDbContext _db;
        private Employee _currentEmployee; 

        public MovementsView()
        {
            InitializeComponent();
            RefreshHistory();
        }

        
        private void RefreshHistory()
        {
            if (_db != null)
                _db.Dispose();
            _db = new AppDbContext();
            MovementsGrid.ItemsSource = _db.MovementHistories
                .Include(m => m.Employee)
                .OrderByDescending(m => m.TimeStamp)
                .ToList();
        }

      
        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = txtCardNumber.Text.Trim();
            if (string.IsNullOrEmpty(cardNumber))
            {
                MessageBox.Show("Введите номер карты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                _currentEmployee = context.Employees
                    .FirstOrDefault(emp => emp.AccessCardNumber == cardNumber);
            }

            if (_currentEmployee != null)
            {
                tbEmployeeInfo.Text = $"{_currentEmployee.FullName} (карта: {_currentEmployee.AccessCardNumber})";
                tbEmployeeInfo.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                tbEmployeeInfo.Text = "Сотрудник с таким номером карты не найден!";
                tbEmployeeInfo.Foreground = System.Windows.Media.Brushes.Red;
                _currentEmployee = null;
            }
        }

    
        private void RegisterMovement(string direction)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Сначала найдите сотрудника по номеру карты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

       
            var movement = new MovementHistory
            {
                EmployeeId = _currentEmployee.Id,
                TimeStamp = DateTime.Now,
                Direction = direction,
                Location = "Главный турникет"
            };

            _db.MovementHistories.Add(movement);
            _db.SaveChanges();

            RefreshHistory();
            MessageBox.Show($"Проход {direction} сотрудника {_currentEmployee.FullName} зафиксирован.",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e) => RegisterMovement("Вход");
        private void BtnExit_Click(object sender, RoutedEventArgs e) => RegisterMovement("Выход");
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => RefreshHistory();
    }
}
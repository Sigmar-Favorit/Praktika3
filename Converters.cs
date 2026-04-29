using System;
using System.Globalization;
using System.Windows.Data;

namespace Praktika3
{
    public class DiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rating)
            {
                if (rating >= 8) return "15%";
                if (rating >= 5) return "10%";
                if (rating >= 3) return "5%";
                return "0%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TotalSalesToDiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal totalSales)
            {
                if (totalSales >= 200000) return "15%";
                if (totalSales >= 50000) return "10%";
                if (totalSales >= 10000) return "5%";
                return "0%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class EmptyOrZeroStringToHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            Visibility v = Visibility.Visible;
            if (value == null || string.Compare(value.ToString(), String.Empty) == 0 || string.Compare(value.ToString(), "0") == 0)
            {
                v = Visibility.Collapsed;
            }
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return null;
        }
    }
}
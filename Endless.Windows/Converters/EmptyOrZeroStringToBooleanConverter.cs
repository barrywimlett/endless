using System;
using System.Globalization;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class EmptyOrZeroStringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            bool r=value == null || string.Compare(value.ToString(), String.Empty) == 0 || string.Compare(value.ToString(), "0") == 0;
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return null;
        }
    }
}
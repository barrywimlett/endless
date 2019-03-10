using System;
using System.Globalization;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return !((bool) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return !((bool)value);
        }
    }
}
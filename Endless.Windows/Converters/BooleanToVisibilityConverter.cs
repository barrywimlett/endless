using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    //Error on BooleanToVisibilityConverter stating does not implement interface member 'System.Windows.Data.IValueConverter.Convert(object, System.Type, object, System.Globalization.CultureInfo)

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return value is Visibility && (Visibility) value == Visibility.Visible;
        }
    }
}

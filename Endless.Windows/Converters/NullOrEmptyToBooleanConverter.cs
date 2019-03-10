using System;
using System.Collections;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class NullOrEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ICollection collection = value as ICollection;
            return collection != null && collection.Count > 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
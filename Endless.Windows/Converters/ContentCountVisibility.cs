using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class ContentCountVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            Visibility v = Visibility.Visible;
            IList list = value as IList;

            if (list == null || list.Count == 0  )
            {
                v = Visibility.Collapsed;
            }
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return Binding.DoNothing;
        }
    }
}
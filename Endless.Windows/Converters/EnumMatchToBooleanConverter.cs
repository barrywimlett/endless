using System;
using System.Globalization;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            var result=checkValue.Equals(targetValue,
                StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return targetValue;

            return Binding.DoNothing;
        }
    }
}
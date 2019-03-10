using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class StringMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Contract.Assert(parameter!= null);

            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            object targetValue = Binding.DoNothing;;
            if (useValue)
            {
                targetValue = parameter.ToString();
            }

            return targetValue;

            
        }
    }
}
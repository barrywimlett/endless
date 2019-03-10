using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class ConverterSequence : IValueConverter
    {
        
        public ConverterSequence()
        {
            Items = new List<ConverterSequenceItem>();

        }

        public List<ConverterSequenceItem> Items { get; set; }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            foreach (var item in Items)
            {
                var param = parameter;
                if (item.ConverterParameter != null)
                {
                    param = item.ConverterParameter;
                }

                value = item.Converter.ConvertBack(value, item.TargetType, param, culture);            
            }

            return value;
        }


        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            foreach (var item in Items)
            {
                var param = parameter;
                if (item.ConverterParameter != null)
                {
                    param = item.ConverterParameter;
                }
                value = item.Converter.Convert(value, item.TargetType, param,culture);
            }
            return value;
        }

    }
}
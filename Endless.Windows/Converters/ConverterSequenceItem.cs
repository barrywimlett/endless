using System;
using System.Windows.Data;

namespace Endless.Windows.Converters
{
    public class ConverterSequenceItem 
    {
        public Binding cx;

        public IValueConverter Converter { get; set; }
        public Type TargetType { get; set; }
        public object ConverterParameter { get; set; }
    }
}
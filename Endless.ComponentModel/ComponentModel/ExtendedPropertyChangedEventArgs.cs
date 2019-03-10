using System;
using System.ComponentModel;

namespace Endless
{
    public class ExtendedPropertyChangedEventArgs :PropertyChangedEventArgs
    {
        public Type ValueType { get; protected set; }
        public object OldValue { get; protected set; }
        public object NewValue { get; protected set; }

        public ExtendedPropertyChangedEventArgs(string propertyName, Type type, object oldValue, object newValue)
            : base(propertyName)
        {
            this.ValueType = type;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}
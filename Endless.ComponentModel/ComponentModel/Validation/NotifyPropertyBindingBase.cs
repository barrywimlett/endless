using System;

namespace Endless.ComponentModel.Validation
{
    public class NotifyPropertyBindingBase<T> : RuleBindingBase<T>
    {

        private NotifyPropertyBindingBase(NotifyProperty<T> property)
        {
            this.NotifyProperty = property;
        }

        public NotifyPropertyBindingBase(NotifyProperty<T> property, Rule<T> rule) :this(property)
        {
            this.Rule = rule;
        
        }

        public NotifyPropertyBindingBase(NotifyProperty<T> property, Func<T, bool> func, object error)
            : base(func, error)
        {
            this.NotifyProperty = property;
        }

        public NotifyProperty<T> NotifyProperty { get; protected set; }

        public override T Value
        {
            get { return this.NotifyProperty.Value; }
        }
    }
}
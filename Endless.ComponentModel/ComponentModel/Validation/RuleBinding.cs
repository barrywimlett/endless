using System;
using System.Diagnostics.Contracts;

namespace Endless.ComponentModel.Validation
{
    public class RuleBinding<T> : RuleBindingBase<T>
    {
        private T _value;
        override public T Value { get { return _value; }}

        public RuleBinding(T value, Rule<T> rule) :base()
        {
            Contract.Assume(rule!=null);

            this.Rule = rule;
            this._value= value;
        }


        public RuleBinding(T value, Func<T,bool> func,object error) : base(func,error)
        {
            this._value = value;
        }
    }
}
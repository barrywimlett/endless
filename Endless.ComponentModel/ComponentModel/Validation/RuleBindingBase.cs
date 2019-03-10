using System;
using System.Collections.Generic;

namespace Endless.ComponentModel.Validation
{
    public abstract class RuleBindingBase<T> : IRuleBinding
    {
        abstract public T Value { get; }
        public Rule<T> Rule { get; protected set; }

        private readonly List<string> mappedProperties= new List<string>();
        public List<string> MappedProperties { get { return mappedProperties; } }

        public bool IsValid
        {
            get
            {
                
                bool isValid=this.Rule.IsValid(this.Value);

                return isValid;
            }
        }

        public RuleBindingBase()
        {
            
        }
        
        public RuleBindingBase(Func<T,bool> func,object error) 
        {
            this.Rule = new DelegateRule<T>(error,func);
        }

        public object Error { get { return Rule.Error; } }
    }
}
using System;
using System.Collections.Generic;

namespace Endless.ComponentModel.Validation
{
    public class PropertyMapping
    {
        public string PropertyName { get; protected set; }
        private IList<IRuleBinding> Bindings { get; set; }
        private readonly IList<IRuleBinding> failingRules= new List<IRuleBinding>();
        public IList<object> Errors { get; protected set; }

        public PropertyMapping(string propertyName)
        {
            this.PropertyName = propertyName;
            this.Errors=new List<object>();
            this.Bindings= new List<IRuleBinding>();
        }

        internal void AddBinding(IRuleBinding binding)
        {
            this.Bindings.Add(binding);
            binding.MappedProperties.Add(this.PropertyName);
        }
        
        public bool Validate()
        {
            bool propertyIsValid = true;
            bool errorsChanged = false;
            List<string> affectedProperties= new List<string>();

            foreach (var  rule in Bindings)
            {
                bool rulePasses = rule.IsValid;

                if (rulePasses == false)
                {
                    propertyIsValid = false;
                    if (!failingRules.Contains(rule))
                    {
                        Errors.Add(rule.Error);
                        failingRules.Add(rule);
                        errorsChanged = true;
                        affectedProperties.AddRange(rule.MappedProperties);
                    }
                }
                else
                {
                    if (failingRules.Contains(rule))
                    {
                        Errors.Remove(rule.Error);
                        failingRules.Remove(rule);
                        errorsChanged = true;
                        affectedProperties.AddRange(rule.MappedProperties);
                    }
                }
            }

            if (errorsChanged)
            {
                OnErrorsChanged(affectedProperties);
            }

            return propertyIsValid;
        }

        private event EventHandler<MappingErrorsChangedEventArgs> errorsChanged;

        public event EventHandler<MappingErrorsChangedEventArgs> ErrorsChanged
        {
            add { this.errorsChanged += value; }
            remove { this.errorsChanged -= value; }
        }

        private void OnErrorsChanged(IList<string> affectedProperties)
        {
            EventHandler<MappingErrorsChangedEventArgs> eventHandler = this.errorsChanged;
            if (eventHandler != null)
            {
                foreach (var propertyName in affectedProperties)
                {
                    bool revalidate = propertyName != this.PropertyName;
                    eventHandler(this, new MappingErrorsChangedEventArgs(propertyName, revalidate));
                }
            }
        }

    }
}
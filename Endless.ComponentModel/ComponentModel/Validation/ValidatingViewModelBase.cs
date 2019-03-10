using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Endless.ComponentModel.Validation
{
    public class ValidatingViewModelBase : ViewModelBase, IValidatingViewModelBase
    {

        private readonly IDictionary<string, PropertyMapping> mappings = new Dictionary<string, PropertyMapping>();

        public IDictionary<string, PropertyMapping> Mappings
        {
            get { return mappings; }
        }

        private IList<INotifyDataErrorInfo> childviewModels = new List<INotifyDataErrorInfo>();


        private IList<IRuleBinding> Bindings { get; set; }
        private readonly IList<IRuleBinding> failingRules = new List<IRuleBinding>();
        public IList<object> Errors { get; protected set; }

        public void AddProperty(INotifyProperty property)
        {
            var mapping = new PropertyMapping(property.Name);
            AddMapping(mapping);
        }

        protected void BindModelRule<T>(string propertyName, string message, Func<T, bool> ruleFunc) where T : class,IViewModelBase
        {
            var rule = new DelegateRule<T>(message, ruleFunc);
            var questionTypeBinding = new RuleBinding<T>(this as T, rule);
            AddModelRuleBinding(questionTypeBinding);
            AddPropertyRuleBinding(propertyName, questionTypeBinding);
        }

        public void AddRule<T>(NotifyProperty<T> property, Rule<T> rule)
        {
            PropertyMapping mapping = null;

            if (mappings.Keys.Contains(property.Name))
            {
                mapping = mappings[property.Name];
            }
            else
            {
                mapping = new PropertyMapping(property.Name);
                AddMapping(mapping);
            }

            mapping.AddBinding(new NotifyPropertyBindingBase<T>(property, rule));
            mapping.Validate();
        }

        private void AddMapping(PropertyMapping mapping)
        {
            Contract.Assume(mappings.Keys.Contains(mapping.PropertyName) == false);
            mappings.Add(mapping.PropertyName, mapping);
            mapping.ErrorsChanged += PropertyErrorsChanged;
        }

        protected void AddModelRuleBinding<T>(RuleBindingBase<T> binding)
        {
            AddPropertyRuleBinding<T>(string.Empty, binding);
        }

        protected void AddPropertyRuleBinding<T>(string propertyName, RuleBindingBase<T> binding)
        {
            PropertyMapping mapping = null;

            Contract.Assume(binding != null);
            Contract.Assume(binding.Rule != null);

            if (mappings.Keys.Contains(propertyName))
            {
                mapping = mappings[propertyName];
            }
            else
            {
                mapping = new PropertyMapping(propertyName);
                AddMapping(mapping);
            }

            mapping.AddBinding(binding);
            mapping.Validate();
        }

        protected internal override void NotifyPropertyChanges(string propertyName)
        {
            base.NotifyPropertyChanges(propertyName);
            ValidateMapping(propertyName);
            ValidateMapping(string.Empty);

        }

        private void ValidateMapping(string propertyName)
        {
            if (mappings.Keys.Contains(propertyName))
            {
                var mapping = mappings[propertyName];
                mapping.Validate();
            }
        }

        private void PropertyErrorsChanged(object sender, MappingErrorsChangedEventArgs e)
        {
            if (e.RequiresRevalidate)
            {
                mappings[e.PropertyName].Validate();
            }

            this.OnErrorsChanged(e.PropertyName);
            base.NotifyPropertyChanges("ValidationErrors");
        }

        private bool disableValidation = false;

        public bool DisableValidation
        {
            get { return disableValidation; }
            set
            {
                disableValidation = value;
                EventHandler<DataErrorsChangedEventArgs> eventHandler = this.errorsChanged;
                if (eventHandler != null)
                {
                    foreach (var mapping in mappings.Values)
                    {
                        eventHandler(this, new DataErrorsChangedEventArgs(mapping.PropertyName));
                    }
                    eventHandler(this, new DataErrorsChangedEventArgs(string.Empty));
                }


            }
        }

        private event EventHandler<DataErrorsChangedEventArgs> errorsChanged;

        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            add { this.errorsChanged += value; }
            remove { this.errorsChanged -= value; }
        }


        public void AddChildViewModel(INotifyDataErrorInfo childviewModel)
        {
            this.childviewModels.Add(childviewModel);
        }

        protected virtual void OnErrorsChanged(string propertyName = null)
        {
            Debug.Assert(
                string.IsNullOrEmpty(propertyName) ||
                (this.GetType().GetRuntimeProperty(propertyName) != null),
                "Check that the property name exists for this instance.");

            EventHandler<DataErrorsChangedEventArgs> eventHandler = this.errorsChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }


        public virtual System.Collections.IEnumerable GetErrors(string propertyName)
        {
            Debug.Assert(
                string.IsNullOrEmpty(propertyName) ||
                (this.GetType().GetRuntimeProperty(propertyName) != null),
                "Check that the property name exists for this instance.");

            IEnumerable result = new List<object>();
            if (DisableValidation)
            {
                // nothing wrong move along nothing to see here

            }
            else if (string.IsNullOrEmpty(propertyName))
            {
                // disable viewmodel errors
#if true
                if (this.mappings.ContainsKey(string.Empty))
                {
                    var mapping = mappings[string.Empty];
                    result = mapping.Errors;
                }
#endif
            }
            else
            {
                if (this.mappings.ContainsKey(propertyName))
                {
                    var mapping = mappings[propertyName];
                    result = mapping.Errors;
                }

            }

            foreach (var r in result)
            {
                Debug.WriteLine(r.ToString());
            }
            return result;
        }

        public virtual bool HasErrors
        {
            get
            {
                bool hasErrors = false;
                if (!DisableValidation)
                {
                    hasErrors = this.mappings.Values.Any(p => p.Errors.Count > 0);
                }

                Debug.WriteLine("HasErrors:{0}", hasErrors);
#if DEBUG
                foreach (var val in this.mappings.Values)
                {
                    foreach (var err in val.Errors)
                    {
                        Debug.WriteLine("HasErrors because:{0} has {1}", val.PropertyName,
                            err);

                    }
                }
#endif
                return hasErrors;

            }

        }

        public List<ValidationError> ValidationErrors
        {
            get
            {
                List<ValidationError> errors = new List<ValidationError>();
#if DEBUG
                foreach (var val in this.mappings.Values)
                {
                    foreach (var err in val.Errors)
                    {
                        errors.Add( new ValidationError(val.PropertyName,err.ToString()));
                    }
                }
#endif
                return errors;

            }

        }

    }
}
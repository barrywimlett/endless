using System.Collections.Generic;
using System.Configuration;

namespace Endless.Messaging.Rabbit.Configuration
{
    public class ConnectionCollection : ConfigurationElementCollection, IEnumerable<ConnectionElement>
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionElement)element).Name;
        }

        public new ConnectionElement this[string name]
        {
            get
            {
                return (!string.IsNullOrEmpty(name)) ? (ConnectionElement)BaseGet(name.ToLower()) : null;
            }
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        IEnumerator<ConnectionElement> IEnumerable<ConnectionElement>.GetEnumerator()
        {
            int count = this.Count;

            for (int i = 0; i < count; i++)
            {
                yield return this.BaseGet(i) as ConnectionElement;
            }
        }

        protected override string ElementName
        {
            get { return "connections"; }
        }
    }
}
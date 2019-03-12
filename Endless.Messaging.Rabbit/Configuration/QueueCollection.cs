using System.Collections.Generic;
using System.Configuration;

namespace Endless.Messaging.Rabbit.Configuration
{
    public class QueueCollection : ConfigurationElementCollection, IEnumerable<QueueElement>
    {
        new public QueueElement this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name) || IndexOf(name.ToLower()) < 0) return null;
                return (QueueElement)BaseGet(name.ToLower());
            }
        }

        public QueueElement this[int index]
        {
            get { return (QueueElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Key.ToLower() == name)
                    return idx;
            }

            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueueElement)element).Key;
        }

        public new IEnumerator<QueueElement> GetEnumerator()
        {
            int count = this.Count;

            for (int i = 0; i < count; i++)
            {
                yield return this.BaseGet(i) as QueueElement;
            }
        }

        protected override string ElementName
        {
            get { return "queue"; }
        }
    }
}

using System;

namespace Endless.Collections
{
    public class CollectionViewFilter<T> where T : class
    {
        private Func<T, bool> func;

        public CollectionViewFilter(Func<T, bool> func)
        {
            this.func = func;
        }

        public bool Filter(object obj)
        {
            bool filter = false;
            T t = obj as T;
            if (t != null)
            {
                filter = func(t);
            }
            return filter;
        }
    }
}
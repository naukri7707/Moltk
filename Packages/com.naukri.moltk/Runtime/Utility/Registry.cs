using System;
using System.Collections.Generic;

namespace Naukri.Moltk.Utility
{
    public class Registry<TItem>
    {
        private readonly Dictionary<Type, TItem> dict = new();

        public bool IsRegistered(TItem item) => dict.ContainsKey(item.GetType());

        public bool Register(TItem item)
        {
            var type = item.GetType();
            if (dict.ContainsKey(type))
            {
                return false;
            }

            dict[type] = item;
            return true;
        }

        public bool Unregister(TItem item)
        {
            return dict.Remove(item.GetType());
        }

        public T Get<T>() where T : TItem
        {
            if (dict.TryGetValue(typeof(T), out var provider))
            {
                return (T)provider;
            }
            return default;
        }

        public bool TryGet<T>(out T item) where T : TItem
        {
            if (dict.TryGetValue(typeof(T), out var value))
            {
                item = (T)value;
                return true;
            }
            item = default;
            return false;
        }
    }
}

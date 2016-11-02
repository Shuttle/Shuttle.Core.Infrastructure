using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class State : IState
    {
        private readonly Dictionary<string, object> _state = new Dictionary<string, object>();

        public void Clear()
        {
            _state.Clear();
        }

        public void Add(object value)
        {
            Guard.AgainstNull(value, "value");

            _state.Add(value.GetType().FullName, value);
        }

        public void Add(string key, object value)
        {
            Guard.AgainstNull(key, "key");

            _state.Add(key, value);
        }

        public void Add<TItem>(TItem value)
        {
            _state.Add(typeof (TItem).FullName, value);
        }

        public void Add<TItem>(string key, TItem value)
        {
            Guard.AgainstNull(key, "key");

            _state.Add(key, value);
        }

        public void Replace(object value)
        {
            Guard.AgainstNull(value, "value");

            var key = value.GetType().FullName;

            _state.Remove(key);
            _state.Add(key, value);
        }

        public void Replace(string key, object value)
        {
            Guard.AgainstNull(key, "key");

            _state.Remove(key);
            _state.Add(key, value);
        }

        public void Replace<TItem>(TItem value)
        {
            var key = typeof (TItem).FullName;

            _state.Remove(key);
            _state.Add(key, value);
        }

        public void Replace<TItem>(string key, TItem value)
        {
            Guard.AgainstNull(key, "key");

            _state.Remove(key);
            _state.Add(key, value);
        }

        public TItem Get<TItem>()
        {
            return Get<TItem>(typeof (TItem).FullName);
        }

        public TItem Get<TItem>(string key)
        {
            Guard.AgainstNull(key, "key");

            if (!Contains(key))
            {
                return default(TItem);
            }

            return (TItem) _state[key];
        }

        public bool Contains(string key)
        {
            Guard.AgainstNull(key, "key");

            return _state.ContainsKey(key);
        }
    }
}
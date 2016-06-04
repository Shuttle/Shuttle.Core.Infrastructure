using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class State<TOwner> : IState<TOwner>
    {
        private readonly TOwner _owner;
        private readonly Dictionary<string, object> _state = new Dictionary<string, object>();

        public State(TOwner owner)
        {
            _owner = owner;
        }

        public void Clear()
        {
            _state.Clear();
        }

        public TOwner Add(object value)
        {
            Guard.AgainstNull(value, "value");

            _state.Add(value.GetType().FullName, value);

            return _owner;
        }

        public TOwner Add(string key, object value)
        {
            Guard.AgainstNull(key, "key");

            _state.Add(key, value);

            return _owner;
        }

        public TOwner Add<TItem>(TItem value)
        {
            _state.Add(typeof (TItem).FullName, value);

            return _owner;
        }

        public TOwner Add<TItem>(string key, TItem value)
        {
            Guard.AgainstNull(key, "key");

            _state.Add(key, value);

            return _owner;
        }

        public TOwner Replace(object value)
        {
            Guard.AgainstNull(value, "value");

            var key = value.GetType().FullName;

            _state.Remove(key);
            _state.Add(key, value);

            return _owner;
        }

        public TOwner Replace(string key, object value)
        {
            Guard.AgainstNull(key, "key");

            _state.Remove(key);
            _state.Add(key, value);

            return _owner;
        }

        public TOwner Replace<TItem>(TItem value)
        {
            var key = typeof (TItem).FullName;

            _state.Remove(key);
            _state.Add(key, value);

            return _owner;
        }

        public TOwner Replace<TItem>(string key, TItem value)
        {
            Guard.AgainstNull(key, "key");

            _state.Remove(key);
            _state.Add(key, value);

            return _owner;
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

    public class State : State<object>
    {
        public State() : base(null)
        {
        }
    }
}
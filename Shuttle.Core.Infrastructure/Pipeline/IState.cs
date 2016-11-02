namespace Shuttle.Core.Infrastructure
{
    public interface IState
    {
        void Clear();
        void Add(object value);
        void Add(string key, object value);
        void Add<TItem>(TItem value);
        void Add<TItem>(string key, TItem value);
        void Replace(object value);
        void Replace(string key, object value);
        void Replace<TItem>(TItem value);
        void Replace<TItem>(string key, TItem value);
        TItem Get<TItem>();
        TItem Get<TItem>(string key);
        bool Contains(string key);
    }
}
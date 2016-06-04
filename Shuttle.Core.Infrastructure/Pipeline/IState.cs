namespace Shuttle.Core.Infrastructure
{
    public interface IState<out TOwner>
    {
        void Clear();
        TOwner Add(object value);
        TOwner Add(string key, object value);
        TOwner Add<TItem>(TItem value);
        TOwner Add<TItem>(string key, TItem value);
        TOwner Replace(object value);
        TOwner Replace(string key, object value);
        TOwner Replace<TItem>(TItem value);
        TOwner Replace<TItem>(string key, TItem value);
        TItem Get<TItem>();
        TItem Get<TItem>(string key);
        bool Contains(string key);
    }
}
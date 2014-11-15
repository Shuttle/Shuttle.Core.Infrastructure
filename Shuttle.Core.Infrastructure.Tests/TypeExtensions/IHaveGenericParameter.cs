namespace Shuttle.Core.Infrastructure.Tests
{
    public interface IHaveGenericParameter<T>
    {
        void DoSomethingUsing(T item);
    }
}
namespace Shuttle.Core.Infrastructure
{
    public interface IRegisterObserverAnd
    {
        IRegisterObserverAnd AndObserver(IObserver observer);
    }
}
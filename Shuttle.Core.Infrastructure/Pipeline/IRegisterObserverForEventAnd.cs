namespace Shuttle.Core.Infrastructure
{
    public interface IRegisterObserverForEventAnd
    {
        IRegisterObserverForEventAnd AndEvent(string name);
    }
}
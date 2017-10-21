namespace Shuttle.Core.Infrastructure
{
    public interface IComponentResolverBootstrap
    {
        void Resolve(IComponentResolver resolver);
    }
}
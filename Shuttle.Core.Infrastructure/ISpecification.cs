namespace Shuttle.Core.Infrastructure
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}
namespace Shuttle.Core.Infrastructure
{
    public interface IProcessor
    {
        void Execute(IThreadState state);
    }
}
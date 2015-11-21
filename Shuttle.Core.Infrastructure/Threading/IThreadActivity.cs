namespace Shuttle.Core.Infrastructure
{
    public interface IThreadActivity
    {
        void Waiting(IThreadState state);
        void Working();
    }
}
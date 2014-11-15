using System.Threading;

namespace Shuttle.Core.Infrastructure
{
    public static class ThreadSleep
    {
		public static void While(int ms, IThreadState state)
        {
            While(ms, state, 5);
        }

        public static void While(int ms, IThreadState state, int step)
        {
            var elapsed = 0;

            while (state.Active && elapsed < ms)
            {
                Thread.Sleep(step);

                elapsed += step;
            }
        }
    }
}
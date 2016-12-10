using System;
using System.Threading;

namespace Shuttle.Core.Infrastructure
{
    public static class ThreadSleep
    {
        public const int MaxStepSize = 1000;

        public static void While(int ms, IThreadState state)
        {
            // step size should be as large as possible,
            // max step size by default
            While(ms, state, MaxStepSize);
        }

        public static void While(int ms, IThreadState state, int step)
        {
            if (ms < 0)
            {
                return;
            }

            if (step < 0 || step > MaxStepSize)
            {
                step = MaxStepSize;
            }

            var end = DateTime.UtcNow.AddMilliseconds(ms);
            
            // sleep time should be:
            // - as large as possible to reduce burden on the os and improve accuracy
            // - less than the max step size to keep the thread responsive to thread state

            var remaining = (int)(end - DateTime.UtcNow).TotalMilliseconds;

            while (state.Active && remaining > 0)
            {
                var sleep = remaining < step ? remaining : step;
                Thread.Sleep(sleep);
                remaining = (int)(end - DateTime.UtcNow).TotalMilliseconds;
            }
        }
    }
}
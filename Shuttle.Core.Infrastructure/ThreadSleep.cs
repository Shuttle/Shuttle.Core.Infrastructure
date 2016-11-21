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
            // don't sleep less than zero
            if (ms < 0)
                return;

            // step size must be positive and less than max step size
            if (step < 0 || step > MaxStepSize)
                step = MaxStepSize;

            // end time
            var end = DateTime.UtcNow.AddMilliseconds(ms);
            
            // sleep time should be 
            // 1) as large as possible to reduce burden on the os and improve accuracy
            // 2) less than the max step size to keep the thread responsive to thread state

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
using System;

namespace Shuttle.Core.Infrastructure
{
    public class ThreadActivity : IThreadActivity
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromMilliseconds(250);
        private readonly TimeSpan[] _durations;

        private int _durationIndex;

        public ThreadActivity(TimeSpan[] durationToSleepWhenIdle)
        {
            Guard.AgainstNull(durationToSleepWhenIdle, "durationToSleepWhenIdle");

            _durations = durationToSleepWhenIdle;
            _durationIndex = 0;
        }

        public ThreadActivity(IThreadActivityConfiguration threadActivityConfiguration)
        {
            Guard.AgainstNull(threadActivityConfiguration, "threadActivityConfiguration");

            _durations = threadActivityConfiguration.DurationToSleepWhenIdle;
            _durationIndex = 0;
        }

        public void Waiting(IThreadState state)
        {
            var ms = (int) GetSleepTimeSpan().TotalMilliseconds;

            ThreadSleep.While(ms, state);
        }

        public void Working()
        {
            _durationIndex = 0;
        }

        private TimeSpan GetSleepTimeSpan()
        {
            if (_durations == null || _durations.Length == 0)
            {
                return DefaultDuration;
            }

            if (_durationIndex >= _durations.Length)
            {
                _durationIndex = _durations.Length - 1;
            }

            return _durations[_durationIndex++];
        }
    }
}
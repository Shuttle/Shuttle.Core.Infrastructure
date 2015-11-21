using System;

namespace Shuttle.Core.Infrastructure
{
    public interface IThreadActivityConfiguration
    {
        TimeSpan[] DurationToSleepWhenIdle { get; set; }
    }
}
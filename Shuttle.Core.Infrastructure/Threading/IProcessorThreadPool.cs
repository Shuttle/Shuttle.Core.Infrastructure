using System;

namespace Shuttle.Core.Infrastructure
{
    public interface IProcessorThreadPool : IDisposable
    {
        void Pause();
        void Resume();
        IProcessorThreadPool Start();
    }
}
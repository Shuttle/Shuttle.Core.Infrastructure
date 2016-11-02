using System;

namespace Shuttle.Core.Infrastructure
{
    public interface IPipeline
    {
        Guid Id { get; }
        bool ExceptionHandled { get; }
        Exception Exception { get; }
        bool Aborted { get; }
        string StageName { get; }
        IPipelineEvent Event { get; }
        IState State { get; }
        void Abort();
        void MarkExceptionHandled();
        bool Execute();
        IPipelineStage RegisterStage(string name);
        IPipelineStage GetStage(string name);
        IPipeline RegisterObserver(IObserver observer);
    }
}
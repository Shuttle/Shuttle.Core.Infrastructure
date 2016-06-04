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
        IState<IPipeline> State { get; }
        IPipeline RegisterObserver(IObserver observer);
        void Abort();
        void MarkExceptionHandled();
        bool Execute();
        IPipelineStage RegisterStage(string name);
        IPipelineStage GetStage(string name);
    }
}
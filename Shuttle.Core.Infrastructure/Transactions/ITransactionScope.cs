using System;

namespace Shuttle.Core.Infrastructure
{
    public interface ITransactionScope : IDisposable
    {
        string Name { get; }
        void Complete();
    }
}
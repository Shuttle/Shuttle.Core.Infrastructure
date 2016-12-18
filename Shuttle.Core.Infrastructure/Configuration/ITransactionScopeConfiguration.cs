using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
    public interface ITransactionScopeConfiguration
    {
        bool Enabled { get; }
        IsolationLevel IsolationLevel { get; }
        int TimeoutSeconds { get; }
    }
}
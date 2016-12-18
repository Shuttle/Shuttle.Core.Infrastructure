using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
    public class TransactionScopeConfiguration : ITransactionScopeConfiguration
    {
        public TransactionScopeConfiguration()
        {
            Enabled = true;
            IsolationLevel = IsolationLevel.ReadCommitted;
            TimeoutSeconds = 30;
        }

        public bool Enabled { get; set; }
        public IsolationLevel IsolationLevel { get; set; }
        public int TimeoutSeconds { get; set; }
    }
}
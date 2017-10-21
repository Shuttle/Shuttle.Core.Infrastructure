using System;
using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
    public class DefaultTransactionScopeFactory : ITransactionScopeFactory
    {
        private readonly ITransactionScope _nullTransactionScope = new NullTransactionScope();

        public DefaultTransactionScopeFactory(bool enabled, IsolationLevel isolationIsolationLevel, TimeSpan timeout)
        {
            Enabled = enabled;
            IsolationLevel = isolationIsolationLevel;
            Timeout = timeout;
        }

        public bool Enabled { get; }
        public IsolationLevel IsolationLevel { get; }
        public TimeSpan Timeout { get; }

        public ITransactionScope Create(string name)
        {
            return Create(name, IsolationLevel, Timeout);
        }

        public ITransactionScope Create(string name, IsolationLevel isolationLevel, TimeSpan timeout)
        {
            return Enabled ? new DefaultTransactionScope(name, IsolationLevel, Timeout) : _nullTransactionScope;
        }

        public ITransactionScope Create()
        {
            return Create(Guid.NewGuid().ToString(), IsolationLevel, Timeout);
        }

        public ITransactionScope Create(IsolationLevel isolationLevel, TimeSpan timeout)
        {
            return Create(Guid.NewGuid().ToString(), isolationLevel, timeout);
        }
    }
}
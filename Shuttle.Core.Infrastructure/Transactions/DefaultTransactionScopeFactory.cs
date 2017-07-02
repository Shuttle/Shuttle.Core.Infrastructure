using System;
using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
	public class DefaultTransactionScopeFactory : ITransactionScopeFactory
	{
	    public bool Enabled { get; private set; }
	    public IsolationLevel IsolationLevel { get; private set;  }
	    public TimeSpan Timeout { get; private set; }
	    private readonly ITransactionScope _nullTransactionScope = new NullTransactionScope();

	    public DefaultTransactionScopeFactory(bool enabled, IsolationLevel isolationIsolationLevel, TimeSpan timeout)
	    {
	        Enabled = enabled;
	        IsolationLevel = isolationIsolationLevel;
	        Timeout = timeout;
	    }

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
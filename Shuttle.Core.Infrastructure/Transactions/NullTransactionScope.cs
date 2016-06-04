namespace Shuttle.Core.Infrastructure
{
	public class NullTransactionScope : ITransactionScope
	{
	    private const string NullTransactionScopeName = "[null transaction scope]";

        public string Name {
	        get { return NullTransactionScopeName; }
	    }

	    public void Complete()
		{
		}

		public void Dispose()
		{
		}
	}
}
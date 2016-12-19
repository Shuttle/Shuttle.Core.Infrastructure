using System.Reflection;
using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
    internal static class Extensions
    {
        public static ITransactionScope GetTransactionScope(this IState state)
        {
            return state.Get<ITransactionScope>("TransactionScope");
        }

        public static void SetTransactionScope(this IState state, ITransactionScope scope)
        {
            state.Replace("TransactionScope", scope);
        }

        public static bool GetTransactionComplete(this IState state)
        {
            return state.Get<bool>("TransactionComplete");
        }

        public static void SetTransactionComplete(this IState state)
        {
            state.Replace("TransactionComplete", true);
        }
    }

    public class TransactionScopeObserver :
        IPipelineObserver<OnStartTransactionScope>,
        IPipelineObserver<OnCompleteTransactionScope>,
        IPipelineObserver<OnDisposeTransactionScope>,
        IPipelineObserver<OnAbortPipeline>,
        IPipelineObserver<OnPipelineException>
    {
        private readonly ITransactionScopeFactory _transactionScopeFactory;

        public TransactionScopeObserver(ITransactionScopeFactory transactionScopeFactory)
        {
            Guard.AgainstNull(transactionScopeFactory, "transactionScopeFactory");

            _transactionScopeFactory = transactionScopeFactory;
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (state.GetTransactionComplete())
            {
                scope.Complete();
            }

            scope.Dispose();

            state.SetTransactionScope(null);
        }

        public void Execute(OnCompleteTransactionScope pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (pipelineEvent.Pipeline.Exception == null || state.GetTransactionComplete())
            {
                scope.Complete();
            }
        }

        public void Execute(OnDisposeTransactionScope pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            scope.Dispose();

            state.SetTransactionScope(null);
        }

        public void Execute(OnStartTransactionScope pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope != null)
            {
                throw new TransactionException(
                    (string.Format(InfrastructureResources.TransactionAlreadyStartedException, GetType().FullName,
                        MethodBase.GetCurrentMethod().Name)));
            }

            scope = _transactionScopeFactory.Create();

            state.SetTransactionScope(scope);
        }

        public void Execute(OnPipelineException pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (state.GetTransactionComplete())
            {
                scope.Complete();
            }

            scope.Dispose();

            state.SetTransactionScope(null);
        }
    }
}
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Saga
{
    public class Saga<T> : IDisposable
    {
        private readonly IStateProvider<T> _stateProvider;
        private readonly ILogger<Saga<T>> _logger;
        private readonly LinkedList<IMicroTransaction<T>> _steps;
        private readonly TransactionContext<T> _initialContext;
        private bool IsCompleted { get; set; }

        public Saga(IStateProvider<T> stateProvider, ILogger<Saga<T>> logger, T initialContext)
        {
            _stateProvider = stateProvider;
            _logger = logger;
            _steps = new LinkedList<IMicroTransaction<T>>();

            var context = new TransactionContext<T>
            {
                Context = initialContext,
                State = TransactionState.Active,
            };
            _stateProvider.SaveContext(context);
            _initialContext = context;
            IsCompleted = false;
        }

        public void AddStep(IMicroTransaction<T> step) => _steps.AddLast(step);

        public async Task Complete()
        {
            var context = _stateProvider.GetContext(_initialContext.TracingCode);

            using (var tx = new TransactionScope())
            {
                for (var step = _steps.First; step != null; step = step.Next)
                {
                    try
                    {
                        await step.Value.Do(context);
                    }
                    catch
                    {
                        await RollBack(step);
                        return;
                    }
                }

                context.State = TransactionState.Committed;
                _stateProvider.SaveContext(context);
                IsCompleted = true;

                tx.Complete();
            }
        }

        private async Task RollBack(LinkedListNode<IMicroTransaction<T>>? step)
        {
            var context = _stateProvider.GetContext(_initialContext.TracingCode);
            using (var tx = new TransactionScope())
            {
                for (; step != null; step = step.Previous)
                {
                    try
                    {
                        await step.Value.Undo(context);
                    }
                    catch
                    {
                        _logger.LogCritical("TransAction Roll Back Failed");
                        return;
                    }
                }

                context.State = TransactionState.Aborted;
                _stateProvider.SaveContext(context);
                tx.Complete();
            }
        }

        public Task RollBack() => RollBack(_steps.Last);

        public void Dispose()
        {
            if (!IsCompleted)
            {
                RollBack();
            }
        }
    }
}
namespace Saga.V2
{
    internal class AbortedState : ITransactionState
    {
        public Task Change(DistributedTransaction transaction)
        {
            return Task.CompletedTask;
        }
    }
}
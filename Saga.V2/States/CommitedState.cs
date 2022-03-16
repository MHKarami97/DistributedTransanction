namespace Saga.V2
{
    internal class CommitedState : ITransactionState
    {
        public Task Change(DistributedTransaction transaction)
        {
            return Task.CompletedTask;
        }
    }
}
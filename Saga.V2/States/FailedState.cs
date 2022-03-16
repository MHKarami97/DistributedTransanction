namespace Saga.V2
{
    internal class FailedState : ITransactionState
    {
        public async Task Change(DistributedTransaction transaction)
        {
            await transaction.RollBack();
            transaction.State = new AbortedState();
        }
    }
}
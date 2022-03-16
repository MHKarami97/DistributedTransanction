namespace Saga.V2
{
    internal class ActiveState : ITransactionState
    {
        public async Task Change(DistributedTransaction transaction)
        {
            try {
                await transaction.Exceute();
                transaction.State = new CommitedState();
            }
            catch {
                transaction.State = new FailedState();
            }
        }
    }
}

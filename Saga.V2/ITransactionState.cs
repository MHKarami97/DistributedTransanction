namespace Saga.V2
{
    public interface ITransactionState
    {
        Task Change(DistributedTransaction transaction);
    }
}

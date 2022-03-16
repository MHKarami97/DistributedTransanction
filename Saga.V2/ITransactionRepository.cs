using Saga.V2;

namespace Saga
{
    public interface ITransactionRepository
    {
        DistributedTransaction GetTransaction(int collaborationId);
        int SaveState(DistributedTransaction transaction);
        int UpdateState(int collaborationId, TransactionState state);
    }
}

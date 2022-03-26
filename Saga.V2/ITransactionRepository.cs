using Saga.V2;

namespace Saga
{
    public interface ITransactionRepository
    {
        DistributedTransaction GetTransactionByCollaborationId(int collaborationId);
        DistributedTransaction GetTransactionById(int id);
        int SaveState(DistributedTransaction transaction);
        int UpdateState(int collaborationId, TransactionState state);
    }
}

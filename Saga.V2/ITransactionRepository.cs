namespace Saga.V2
{
    public interface ITransactionRepository
    {
        DistributedTransaction GetTransactionByCollaborationId(int collaborationId);
        DistributedTransaction GetTransactionById(int id);
        int SaveState(DistributedTransaction transaction);
        int UpdateByCollaborationId(DistributedTransaction transaction);
        int UpdateState(int collaborationId, TransactionState state);
    }
}

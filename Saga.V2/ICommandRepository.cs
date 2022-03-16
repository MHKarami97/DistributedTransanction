using Saga.V2;

namespace Saga
{
    public interface ICommandRepository
    {
        DistributedTransaction GetTransaction(int collaborationId);
        int SaveState(DistributedTransaction transaction);
        int UpdateState(int collaborationId, ITransactionState state);
    }
}

using System.Transactions;

namespace Saga.V2
{
    public class DistributedTransaction
    {
        public int CollaborationId { get; private set; }
        public ITransationalCommand Command { get; private set; }
        private readonly ITransactionRepository _repository;

        public TransactionState State { get; internal set; } = TransactionState.Active;

        public DistributedTransaction(ITransationalCommand command, ITransactionRepository repository)
        {
            Command = command;
            _repository = repository;
            Init();
        }

        public DistributedTransaction(ITransationalCommand command, ITransactionRepository repository, int collaborationId)
        {
            Command = command;
            _repository = repository;
            CollaborationId = collaborationId;
            Command.CollaborationId = collaborationId;
        }

        private void Init()
        {
            CollaborationId = _repository.SaveState(this);
            Command.CollaborationId = CollaborationId;
        }

        public async Task Begin()
        {
            try
            {
                await Command.Do();
                State = TransactionState.Committed;
                _repository.UpdateState(CollaborationId, State);
            }
            catch
            {
                await Command.Undo();
                State = TransactionState.Aborted;
                using (var tx = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled)) 
                {
                    _repository.UpdateState(CollaborationId, State);
                }
                throw;
            }
        }
    }
}
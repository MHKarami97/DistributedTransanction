using System.Transactions;

namespace Saga.V2
{
    public class DistributedTransaction
    {
        public int CollaborationId { get; private set; }
        public TransationalCommand Command { get; private set; }
        private readonly ITransactionRepository _repository;

        public TransactionState State { get; internal set; } = TransactionState.Active;

        public DistributedTransaction(TransationalCommand command, ITransactionRepository repository)
        {
            _repository = repository;
            Command = command;
            Init();
        }

        public DistributedTransaction(TransationalCommand command, ITransactionRepository repository, int collaborationId)
        {
            _repository = repository;
            Command = command;
            CollaborationId = collaborationId;
            Command.CollaborationId = collaborationId;
            _repository.SaveState(this);
        }

        private DistributedTransaction(int collaborationId, TransationalCommand command, ITransactionRepository repository)
        {
            _repository = repository;
            Command = command;
            CollaborationId = collaborationId;
            Command.CollaborationId = collaborationId;
        }

        private void Init()
        {
            CollaborationId = _repository.SaveState(this);
            Command.CollaborationId = CollaborationId;
        }

        public static DistributedTransaction Load(TransationalCommand command, ITransactionRepository repository, int collaborationId)
        {
            return new DistributedTransaction(collaborationId, command, repository);
        }

        public async Task Execute()
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
                    tx.Complete();
                }
                throw;
            }
        }

        public async Task Compensate()
        {
            if (State == TransactionState.Committed || State == TransactionState.Aborted)
                return;

            try
            {
                await Command.Undo();
                State = TransactionState.Aborted;
                _repository.UpdateState(CollaborationId, State);
            }
            catch
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                {
                    State = TransactionState.Failed;
                    _repository.UpdateState(CollaborationId, State);
                    tx.Complete();
                }
                throw;
            }
        }
    }
}
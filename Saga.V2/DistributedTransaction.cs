namespace Saga.V2
{
    public class DistributedTransaction
    {
        private int _collaborationId;
        private readonly ITransationalCommand _command;
        private readonly ICommandRepository _repository;

        public ITransactionState State { get; internal set; } = new ActiveState();

        public DistributedTransaction(ITransationalCommand command, ICommandRepository repository)
        {
            _command = command;
            _repository = repository;
            Init();
        }

        private void Init()
        {
            _collaborationId = _repository.SaveState(this);
        }

        internal Task Exceute() => _command.Do();
        internal Task RollBack() => _command.Undo();

        public Task Complete() => State.Change(this);
    }
}
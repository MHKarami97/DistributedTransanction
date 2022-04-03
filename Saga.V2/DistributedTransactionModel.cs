using System.Text.Json;

namespace Saga.V2
{
    public class DistributedTransactionModel
    {
        private DistributedTransactionModel()
        {
        }

        public DistributedTransactionModel(DistributedTransaction transaction)
        {
            CollaborationId = transaction.CollaborationId;
            CommandType = transaction.Command.GetType().AssemblyQualifiedName ?? "";
            CommandBody = JsonSerializer.Serialize((object)transaction.Command);
            State = transaction.State;
            StartDateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public int CollaborationId { get; set; }
        public string CommandType { get; set; }
        public string CommandBody { get; set; }
        public TransactionState State { get; set; }
        public DateTime StartDateTime { get; set; }

        public DistributedTransaction Load(IServiceProvider serviceProvider, ITransactionRepository repository) 
        {
            var commandType = Type.GetType(CommandType);

            if (commandType is null)
            {
                throw new Exception("Command Not Found");
            }

            if (JsonSerializer.Deserialize(CommandBody, commandType) is TransactionalCommand command)
            {
                command.ServiceProvider = serviceProvider;
                var distributedTransaction = DistributedTransaction.Load(command, repository, CollaborationId);
                return distributedTransaction;
            }

            throw new Exception("Command Not Found");
        }
    }
}

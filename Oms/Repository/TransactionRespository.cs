using Oms.Context;
using Saga;
using Saga.V2;
using System.Text.Json;

namespace Oms.Repository
{
    public class TransactionRespository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public TransactionRespository(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public DistributedTransaction GetTransaction(int collaborationId)
        {
            var transaction = _context.Set<DistributedTransactionModel>().FirstOrDefault(x => x.CollaborationId == collaborationId);

            if (transaction == null)
                throw new Exception("Transaction Not Found");

            var commandType = Type.GetType(transaction.CommandType);

            if (commandType is null)
            {
                throw new Exception("Command Not Found");
            }

            if (JsonSerializer.Deserialize(transaction.CommandBody, commandType) is ITransationalCommand command)
            {
                command.ServiceProvider = _serviceProvider;
                var distributedTransaction = new DistributedTransaction(command, this, collaborationId);
                return distributedTransaction;
            }

            throw new Exception("Command Not Found");
        }

        public int SaveState(DistributedTransaction transaction)
        {
            _context.Attach(transaction);
            return _context.SaveChanges();
        }

        public int UpdateState(int collaborationId, TransactionState state)
        {
            var transaction = _context.Set<DistributedTransactionModel>().FirstOrDefault(x => x.CollaborationId == collaborationId);

            if (transaction == null)
            {
                return 0;
            }

            transaction.State = state;
            _context.Update(transaction);
            return _context.SaveChanges();
        }
    }
}

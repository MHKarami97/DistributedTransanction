using Accounting.Context;
using Saga;
using Saga.V2;

namespace Accounting.Repository
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

            if (transaction is null)
            {
                throw new NullReferenceException("Transaction Not Found");
            }
            var commandType = Type.GetType(transaction.CommandType);
            return transaction.Load(_serviceProvider, this);
        }

        public int SaveState(DistributedTransaction transaction)
        {
            var model = new DistributedTransactionModel(transaction);
            _context.Attach(model);
            _context.SaveChanges();
            return model.Id;
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

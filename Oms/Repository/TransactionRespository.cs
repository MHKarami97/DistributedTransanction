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

        public DistributedTransaction GetTransactionByCollaborationId(int collaborationId)
        {
            var transaction = _context.Set<DistributedTransactionModel>().FirstOrDefault(x => x.CollaborationId == collaborationId);
            if (transaction is null) 
            {
                throw new NullReferenceException("Transaction Not Found");
            }
            return transaction.Load(_serviceProvider, this);
        }

        public DistributedTransaction GetTransactionById(int id)
        {
            var transaction = _context.Set<DistributedTransactionModel>().Find(id);
            if (transaction is null)
            {
                throw new NullReferenceException("Transaction Not Found");
            }
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

using Accounting.Context;
using Saga.V2;

namespace Accounting.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public TransactionRepository(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public DistributedTransaction GetTransactionByCollaborationId(int collaborationId)
        {
            var transaction = _context.Set<DistributedTransactionModel>()
                .FirstOrDefault(x => x.CollaborationId == collaborationId);

            if (transaction is null)
            {
                throw new NullReferenceException("Transaction Not Found");
            }

            //var commandType = Type.GetType(transaction.CommandType);
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

        public int UpdateByCollaborationId(DistributedTransaction transaction)
        {
            var model = _context.Set<DistributedTransactionModel>()
                .FirstOrDefault(x => x.CollaborationId == transaction.CollaborationId);

            if (model is null)
            {
                throw new NullReferenceException("Transaction Not Found");
            }

            var newModel = new DistributedTransactionModel(transaction);
            model.State = newModel.State;
            model.CommandType = newModel.CommandType;
            model.CommandBody = newModel.CommandBody;

            return _context.SaveChanges();
        }

        public int UpdateState(int collaborationId, TransactionState state)
        {
            var transaction = _context.Set<DistributedTransactionModel>()
                .FirstOrDefault(x => x.CollaborationId == collaborationId);

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
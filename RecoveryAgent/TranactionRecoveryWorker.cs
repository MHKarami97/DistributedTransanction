using Microsoft.Extensions.Hosting;
using Oms.Context;
using Oms.Services;
using Saga.V2;

namespace RecoveryAgent
{
    internal class TranactionRecoveryWorker : BackgroundService
    {
        private readonly ApplicationDbContext _context;
        private Timer _timer;
        const int TIMEOUT_IN_SECONDS = 60;

        public TranactionRecoveryWorker(ApplicationDbContext context)
        {
            _context = context;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _timer = new Timer(Recover, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        void Recover(object? state)
        {
            var suspendedTransactions = _context.Set<DistributedTransactionModel>()
                .Where(t => t.StartDateTime < DateTime.Now.AddSeconds(-TIMEOUT_IN_SECONDS)
                    && (t.State == TransactionState.Active || t.State == TransactionState.Failed))
                .ToList();

            foreach (var transaction in suspendedTransactions) 
            {
                Console.WriteLine("Undo Applied For " + transaction.Id);
                HttpService.Post<bool>("http://localhost:5002/Order/Undo/"+transaction.Id)
                    .ConfigureAwait(false);
            }
        }
    }
}

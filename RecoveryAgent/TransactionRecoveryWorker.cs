using Microsoft.Extensions.Hosting;
using Oms.Context;
using Oms.Services;
using Saga.V2;

namespace RecoveryAgent;

internal class TransactionRecoveryWorker : BackgroundService
{
    private Timer _timer;
    private const int TimeoutInSeconds = 60;
    private readonly ApplicationDbContext _context;

    public TransactionRecoveryWorker(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        _timer = new Timer(Recover, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    private void Recover(object? state)
    {
        var suspendedTransactions = _context.Set<DistributedTransactionModel>()
            .Where(t => t.StartDateTime < DateTime.Now.AddSeconds(-TimeoutInSeconds)
                        && (t.State == TransactionState.Active || t.State == TransactionState.Failed))
            .ToList();

        foreach (var transaction in suspendedTransactions)
        {
            Console.WriteLine("Undo Applied For " + transaction.Id);

            HttpService.Post<bool>("http://localhost:5002/Order/Undo/" + transaction.Id)
                .Wait();
        }
    }

    public override void Dispose()
    {
        _timer.Dispose();
        base.Dispose();
    }
}
using Microsoft.Extensions.Hosting;
using Oms.Context;
using Oms.Services;
using Saga.V2;

namespace RecoveryAgent;

internal class TransactionRecoveryWorker : BackgroundService
{
    private const int _timeoutInSeconds = 60;
    private const int _delayTimeInSecond = 10;
    private readonly ApplicationDbContext _context;

    public TransactionRecoveryWorker(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Recover(cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(_delayTimeInSecond), cancellationToken);
        }
    }

    private async Task Recover(CancellationToken cancellationToken)
    {
        var suspendedTransactions = _context.Set<DistributedTransactionModel>()
            .Where(t => t.StartDateTime < DateTime.Now.AddSeconds(-_timeoutInSeconds)
                        && (t.State == TransactionState.Active || t.State == TransactionState.Failed))
            .Take(10)
            .ToList();

        foreach (var transaction in suspendedTransactions)
        {
            Console.WriteLine("Undo Applied For " + transaction.Id);

            await HttpService.Post<bool>("http://localhost:5002/Order/Undo/" + transaction.Id,
                cancellationToken: cancellationToken);
        }
    }
}
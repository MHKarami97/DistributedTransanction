using Microsoft.AspNetCore.Mvc;
using Oms.Commands;
using Saga;
using Saga.V2;
using System.Transactions;

namespace Oms.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger
        , IServiceProvider serviceProvider
        , ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _transactionRepository = transactionRepository;
    }

    [HttpPost]
    public async Task<bool> Make(string productName, int quantiry, int price, int customerId)
    {
        var command = new InitialRequestCommmand(productName, quantiry, price, customerId, _serviceProvider);

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
        };

        using (var tx = new TransactionScope(TransactionScopeOption.Required, options,
                   TransactionScopeAsyncFlowOption.Enabled))
        {
            var commander = new DistributedTransaction(command, _transactionRepository);
            try
            {
                await commander.Execute();
                tx.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        return true;
    }

    [HttpPost("Undo/{collaborationId}")]
    public async Task<bool> Undo(int collaborationId)
    {
        var commander = _transactionRepository.GetTransactionById(collaborationId);

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
        };

        using (var tx = new TransactionScope(TransactionScopeOption.Required, options,
                   TransactionScopeAsyncFlowOption.Enabled))
        {   
            try
            {
                await commander.Compensate();
                tx.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        return true;
    }
}
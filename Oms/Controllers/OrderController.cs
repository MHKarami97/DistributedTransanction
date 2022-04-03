using Microsoft.AspNetCore.Mvc;
using Oms.Commands;
using Oms.Models;
using Saga.V2;
using System.Transactions;

namespace Oms.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITransactionRepository _transactionRepository;

    public OrderController(ILogger<OrderController> logger,
        IServiceProvider serviceProvider,
        ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _transactionRepository = transactionRepository;
    }

    [HttpPost]
    public async Task<bool> Make(MakeModel model)
    {
        var command = new InitialRequestCommand(model.ProductName, model.Quantity, model.Price, model.CustomerId, _serviceProvider) ;

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
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
                _logger.LogError(ex, "Exception on make order, ProductName: {item1} ", model.ProductName);

                return false;
            }
        }

        Thread.Sleep(3000);

        return true;
    }

    [HttpPost("Undo/{collaborationId:int}")]
    public async Task<bool> Undo(int collaborationId)
    {
        var commander = _transactionRepository.GetTransactionById(collaborationId);

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
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
                _logger.LogError(ex, "Exception on undo, CollaborationId: {item1}", collaborationId);

                return false;
            }
        }

        Thread.Sleep(3000);

        return true;
    }
}
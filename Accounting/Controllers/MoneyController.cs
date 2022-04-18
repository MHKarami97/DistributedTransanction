using Accounting.Commands;
using Accounting.Models;
using Microsoft.AspNetCore.Mvc;
using Saga.V2;
using System.Transactions;

namespace Accounting.Controllers;

[ApiController]
[Route("[controller]")]
public class MoneyController : ControllerBase
{
    private readonly ILogger<MoneyController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITransactionRepository _transactionRepository;

    public MoneyController(ILogger<MoneyController> logger,
        ITransactionRepository transactionRepository,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _transactionRepository = transactionRepository;
    }

    [HttpPost("Block")]
    public async Task<bool> Block(BlockModel model)
    {
        var command = new BlockCommand(model.CustomerId, model.Amount, _serviceProvider);

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        };

        var commander = new DistributedTransaction(command, _transactionRepository, model.CollaborationId);

        using (var tx = new TransactionScope(TransactionScopeOption.Required, options,
                   TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await commander.Execute();
                tx.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception on make order, CollaborationId: {item1} ", model.CollaborationId);

                return false;
            }
        }

        return true;
    }

    [HttpPost("Block")]
    public async Task<bool> Decrease(DecreaseBlockModel model)
    {
        try
        {
            var command = new DecreaseBlockCommand(model.BlockId, model.Amount, _serviceProvider);

            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            var commander = new DistributedTransaction(command, _transactionRepository, model.CollaborationId);

            using var tx = new TransactionScope(TransactionScopeOption.Required, options,
                TransactionScopeAsyncFlowOption.Enabled);

            await commander.Execute();

            tx.Complete();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception on make order, CollaborationId: {item1} ", model.CollaborationId);

            return false;
        }

        return true;
    }

    [HttpPost("Undo/{collaborationId:int}")]
    public async Task<bool> Undo(int collaborationId)
    {
        try
        {
            var tx = _transactionRepository.GetTransactionByCollaborationId(collaborationId);

            try
            {
                await tx.Compensate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception on make order, CollaborationId: {item1} ", collaborationId);

                return false;
            }

            return true;
        }
        catch
        {
            return true;
        }
    }
}
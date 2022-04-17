using System.Transactions;
using BackOffice.Commands;
using BackOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Saga.V2;

namespace BackOffice.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly ILogger<TradeController> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITransactionRepository _transactionRepository;

    public TradeController(ILogger<TradeController> logger,
        ITransactionRepository transactionRepository,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _transactionRepository = transactionRepository;
    }

    [HttpPost]
    public async Task<bool> SaveTrade(TradeModel model)
    {
        try
        {
            var command = new SaveTradeCommand(model.CustomerId, model.TradePrice, model.TradedQuantity,
                model.OrderSide, model.InstrumentName, _serviceProvider);

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
            _logger.LogError(ex, "Exception on save trade, CollaborationId: {item1} ", model.CollaborationId);

            return false;
        }

        return true;
    }
    
    [HttpPost("Undo/{collaborationId:int}")]
    public async Task<bool> UndoSaveTrade(int collaborationId)
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
                _logger.LogError(ex, "Exception on undo SaveTrade, CollaborationId: {item1} ", collaborationId);

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
using Microsoft.AspNetCore.Mvc;
using Oms.Context;
using Oms.Models;
using System.Transactions;

namespace Oms.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public bool AddRequest(string productName, int quantiry, int price)
    {
        var request = new Request
        {
            Price = price,
            Quantity = quantiry,
            ProductName = productName,
            RequestState = RequestState.Pending,
            RequestType = RequestType.Initial
        };

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
        };

        using (var tx = new TransactionScope(TransactionScopeOption.Suppress, options,
                   TransactionScopeAsyncFlowOption.Enabled))
        {
        }

        //Call CAS

        /*
         result = CasService.Block(10);
         if(result.IsSucceded){
             SendToOrderRouter();
         }
         else
         {
             RevertAddRequestToDataBase();
         }
        */

        //SendToOrderRouter

        return true;
    }
}
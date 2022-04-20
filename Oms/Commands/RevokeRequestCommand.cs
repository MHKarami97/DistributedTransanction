using Oms.Context;
using Oms.Models;
using Oms.Models.Enums;
using Oms.Repository;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class RevokeRequestCommand : TransactionalCommand
    {
        public RevokeRequestCommand(int requestId, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            RequestId = requestId;
        }

        public int RequestId { get; }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
            var repository = ServiceProvider.GetRequiredService<RequestRepository>();
            var prevRequest = repository.GetLastRequestById(RequestId);

            if (prevRequest == null) 
            {
                throw new NullReferenceException(nameof(prevRequest));
            }

            var newRequest = new Request
            {
                Price = 0,
                InstrumentName = prevRequest.InstrumentName,
                Quantity = 0,
                OriginCode = RequestId,
                RequestState = RequestState.Pending,
                RequestType = RequestType.Revoke
            };
            
            context.Add(newRequest);
            await context.SaveChangesAsync();

            var result = await HttpService.Post<AccountingResult>("http://localhost:5000/money/decrease", new
            {
                CollaborationId,
                prevRequest.BlockCode,
                Amount = - prevRequest.Quantity * prevRequest.Price,
            });

            if (!result.IsSucceded)
            {
                throw new Exception("BlockFailed");
            }

            newRequest.RequestState = RequestState.Completed;

            context.Update(newRequest); // Insert for performance
            await context.SaveChangesAsync();
        }

        public override async Task Undo()
        {
            var undoResult = await HttpService.Post<bool>("http://localhost:5000/money/undo/" + CollaborationId);

            if (!undoResult) {
                throw new Exception("Block Undo Failed");
            }
        }
    }
}

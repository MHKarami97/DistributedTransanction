using Oms.Context;
using Oms.Models;
using Oms.Models.Enums;
using Oms.Repository;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class ChangeRequestCommand : TransactionalCommand
    {
        public ChangeRequestCommand(int requestId, string instrumentName, int quantity, int price, int customerId, IServiceProvider serviceProvider)
        {
            RequestId = requestId;
            InstrumentName = instrumentName;
            Quantity = quantity;
            Price = price;
            CustomerId = customerId;
            ServiceProvider = serviceProvider;
        }

        public int Price { get; }
        public int CustomerId { get; }
        public int Quantity { get; }
        public int RequestId { get; }
        public string InstrumentName { get; }

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
                Price = Price,
                OriginCode = RequestId,
                InstrumentName = prevRequest.InstrumentName,
                Quantity = Quantity,
                RequestState = RequestState.Pending,
                RequestType = RequestType.Revoke
            };

            context.Add(newRequest);
            await context.SaveChangesAsync();

            var changeAmount = prevRequest.Quantity * prevRequest.Price - Quantity * Price;

            var result = await HttpService.Post<AccountingResult>("http://localhost:5000/money/decrease", new
            {
                CollaborationId,
                prevRequest.BlockCode,
                Amount = changeAmount,
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

            if (!undoResult)
            {
                throw new Exception("Block Undo Failed");
            }
        }
    }
}

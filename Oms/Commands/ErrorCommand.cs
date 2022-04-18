using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class ErrorCommand : TransactionalCommand
    {
        public ErrorCommand(
            long amount,
            IServiceProvider serviceProvider)
        {
            Amount = amount;
            ServiceProvider = serviceProvider;
        }

        public long Amount { get; }

        public override async Task Do()
        {
            // var request = new Request
            // {
            //     Price = Price,
            //     instrumentName = InstrumentName,
            //     Quantity = Quantity,
            //     RequestState = RequestState.Pending,
            //     RequestType = RequestType.Initial
            // };
            //
            // var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // context.Add(request);
            // await context.SaveChangesAsync();
            //
            // var result = await HttpService.Post<bool>("http://localhost:5000/money/block", new
            // {
            //     CustomerId,
            //     CollaborationId,
            //     Amount = Price * Quantity,
            // });
            //
            // if (!result)
            // {
            //     throw new Exception("BlockFailed");
            // }
            //
            // request.RequestState = RequestState.Completed;
            //
            // context.Update(request); // Insert for performance
            // await context.SaveChangesAsync();
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
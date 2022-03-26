using Oms.Context;
using Oms.Models;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class InitialRequestCommmand : TransationalCommand
    {
        public InitialRequestCommmand(string productName, int quantity, int price, int customerId, IServiceProvider serviceProvider)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            CustomerId = customerId;
            ServiceProvider = serviceProvider;
        }

        public int Price { get; }
        public int CustomerId { get; }
        public int Quantity { get; }
        public string ProductName { get; }

        public override async Task Do()
        {
            var request = new Request
            {
                Price = Price,
                ProductName = ProductName,
                Quantity = Quantity,
                RequestState = RequestState.Pending,
                RequestType = RequestType.Initial
            };
            
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Add(request);
            context.SaveChanges();

            var result = await HttpService.Post<bool>("http://localhost:5000/money/block", new
            {
                CustomerId,
                CollaborationId,
                Amount = Price * Quantity,
            });

            if (!result)
            {
                throw new Exception("BlockFailed");
            }

            request.RequestState = RequestState.Completed;

            context.Update(request);
            context.SaveChanges();
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

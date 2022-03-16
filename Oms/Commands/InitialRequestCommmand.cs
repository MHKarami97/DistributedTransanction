using Oms.Context;
using Oms.Models;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class InitialRequestCommmand : ITransationalCommand
    {
        public InitialRequestCommmand(string productName, int quantity, int price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public int Price { get; }
        public int Quantity { get; }
        public string ProductName { get; }
        public int CollaborationId { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public Task Do()
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

            var httpService = ServiceProvider.GetRequiredService<HttpService>();
            httpService.Post("cas/block", new
            {
                CollaborationId,
                Amount = Price * Quantity
            });

            request.RequestState = RequestState.Completed;

            context.Update(request);
            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task Undo()
        {
            var httpService = ServiceProvider.GetRequiredService<HttpService>();
            httpService.Post("cas/unblock", new
            {
                CollaborationId,
            });

            return Task.CompletedTask;
        }
    }
}

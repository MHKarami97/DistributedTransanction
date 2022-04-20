using Oms.Context;
using Oms.Models;
using Oms.Models.Enums;
using Saga.V2;

namespace Oms.Commands
{
    public class ErrorCommand : TransactionalCommand
    {
        public ErrorCommand(int requestId,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            RequestId = requestId;
        }

        public int RequestId { get; }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //todo with id pass?
            var request = await context.FindAsync<Request>(RequestId);

            if (request == null)
            {
                throw new Exception("not found request");
            }

            request.RequestState = RequestState.ClosedByError;

            context.Update(request);

            await context.SaveChangesAsync();
        }

        public override Task Undo()
        {
            //do nothing
            return Task.CompletedTask;
        }
    }
}
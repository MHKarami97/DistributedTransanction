using Oms.Context;
using Oms.Models;
using Oms.Models.Enums;
using Saga.V2;

namespace Oms.Commands
{
    public class ErrorCommand : TransactionalCommand
    {
        public ErrorCommand(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //todo with id pass?
            var request = await context.FindAsync<Request>(CollaborationId);

            if (request == null)
            {
                throw new Exception("not found request");
            }

            request.RequestState = RequestState.ClosedByError;

            context.Update(request);

            await context.SaveChangesAsync();
        }

        public override async Task Undo()
        {
        }
    }
}
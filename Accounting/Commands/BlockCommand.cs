using Accounting.Context;
using Accounting.Models;
using Saga.V2;

namespace Accounting.Commands
{
    class BlockCommand : TransationalCommand
    {
        public int BlockId { get; set; }
        public int CustomerId { get; }
        public int Amount { get; }

        public BlockCommand(int customerId, int amount, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CustomerId = customerId;
            Amount = amount;
        }

        public async override Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var block = new Block
            {
                CustomerId = CustomerId,
                Amount = Amount,
                CreateDatetime = DateTime.UtcNow,
            };

            context.Add(block);

            await context.SaveChangesAsync();

            BlockId = block.Id;
        }

        public override async Task Undo()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var block = await context.Set<Block>().FindAsync(BlockId);

            if (block is null) 
            {
                return;
            }

            context.Remove(block);
            await context.SaveChangesAsync();
        }
    }
}

using Accounting.Context;
using Accounting.Models;
using Saga.V2;

namespace Accounting.Commands
{
    class DecreaseBlockCommand : TransactionalCommand
    {
        public int BlockId { get; set; }
        public int Amount { get; }

        public DecreaseBlockCommand(int blockId, int amount, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            BlockId = blockId;
            Amount = amount;
        }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var block = await context.FindAsync<Block>(BlockId);

            if (block == null)
            {
                throw new Exception("not found block");
            }

            block.Amount -= Amount;

            context.Update(block);

            await context.SaveChangesAsync();
        }

        public override async Task Undo()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var block = await context.Set<Block>().FindAsync(BlockId);

            if (block is null)
            {
                return;
            }

            block.Amount += Amount;

            context.Update(block);

            await context.SaveChangesAsync();
        }
    }
}
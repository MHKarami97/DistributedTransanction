using BackOffice.Context;
using BackOffice.Models;
using BackOffice.Models.Enums;
using Saga.V2;

namespace BackOffice.Commands
{
    public class SaveTradeCommand : TransactionalCommand
    {
        public SaveTradeCommand(int customerId, long tradePrice, int tradedQuantity,
            Side orderSide, string instrumentName, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CustomerId = customerId;
            TradePrice = tradePrice;
            TradedQuantity = tradedQuantity;
            InstrumentName = instrumentName;
            OrderSide = orderSide;
        }

        public int TradeId { get; set; }
        public long TradePrice { get; }
        public int CustomerId { get; }
        public int TradedQuantity { get; }
        public string InstrumentName { get; }
        public Side OrderSide { get; }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var trade = new Trade
            {
                CustomerId = CustomerId,
                CollaborationId = CollaborationId,
                InstrumentName = InstrumentName,
                OrderSide = OrderSide,
                TradedQuantity = TradedQuantity,
                TradePrice = TradePrice
            };

            context.Add(trade);

            await context.SaveChangesAsync();

            TradeId = trade.Id;
        }

        public override async Task Undo()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var block = await context.Set<Trade>().FindAsync(TradeId);

            if (block is null)
            {
                return;
            }

            context.Remove(block);
            await context.SaveChangesAsync();
        }
    }
}
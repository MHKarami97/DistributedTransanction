using Oms.Context;
using Oms.Models;
using Oms.Models.Enums;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class TradeResponseCommand : TransactionalCommand
    {
        public TradeResponseCommand(string instrumentName,
            int tradedQuantity,
            long tradePrice,
            int customerId,
            Side orderSide,
            IServiceProvider serviceProvider)
        {
            InstrumentName = instrumentName;
            TradedQuantity = tradedQuantity;
            TradePrice = tradePrice;
            CustomerId = customerId;
            OrderSide = orderSide;
            ServiceProvider = serviceProvider;
        }

        public long TradePrice { get; }
        public int CustomerId { get; }
        public int TradedQuantity { get; }
        public string InstrumentName { get; }
        public Side OrderSide { get; }

        public override async Task Do()
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var response = new Response
            {
                CustomerId = CustomerId,
                InstrumentName = InstrumentName,
                OrderSide = OrderSide,
                TradedQuantity = TradedQuantity,
                TradePrice = TradePrice,
                ResponseState = ResponseState.Pending
            };

            context.Add(response);

            await context.SaveChangesAsync();

            var result = await HttpService.Post<bool>("http://localhost:59649/trade/savetrade", new
            {
                TradePrice,
                TradedQuantity,
                CustomerId,
                OrderSide,
                InstrumentName,
                CollaborationId
            });

            if (!result)
            {
                throw new Exception("save trade to bof failed");
            }

            response.ResponseState = ResponseState.Completed;

            context.Update(response);

            await context.SaveChangesAsync();
        }

        public override async Task Undo()
        {
            var undoResult =
                await HttpService.Post<bool>("http://localhost:59649/trade/undosavetrade/" + CollaborationId);

            if (!undoResult)
            {
                throw new Exception("undo save trade failed");
            }
        }
    }
}
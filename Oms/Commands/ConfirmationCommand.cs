using Oms.Models.Enums;
using Oms.Services;
using Saga.V2;

namespace Oms.Commands
{
    public class ConfirmationCommand : TransactionalCommand
    {
        public ConfirmationCommand(
            int requestId,
            long? blockedAmountChange,
            long? remainingBlockedAmount,
            OrderStatus orderStatus,
            RequestType requestType,
            RequestStatus requestStatus,
            IServiceProvider serviceProvider)
        {
            RequestId = requestId;
            BlockedAmountChange = blockedAmountChange;
            RemainingBlockedAmount = remainingBlockedAmount;
            OrderStatus = orderStatus;
            RequestType = requestType;
            RequestStatus = requestStatus;
            ServiceProvider = serviceProvider;
        }


        public int RequestId { get; }
        public long? RemainingBlockedAmount { get; }
        public long? BlockedAmountChange { get; }
        public OrderStatus OrderStatus { get; }
        public RequestType RequestType { get; }
        public RequestStatus RequestStatus { get; }

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

            bool result;

            if (RequestType == RequestType.Change)
            {
                var temp = await HandleChangeRequestAmount();

                var temp1 = await HandleAmount();

                result = temp & temp1;
            }
            else
            {
                result = await HandleAmount();
            }

            if (!result)
            {
                throw new Exception("BlockFailed");
            }

            // request.RequestState = RequestState.Completed;
            //
            // context.Update(request); // Insert for performance
            // await context.SaveChangesAsync();
        }

        public override async Task Undo()
        {
            var undoResult = await HttpService.Post<bool>("http://localhost:5000/money/undo/" + CollaborationId);

            if (!undoResult)
            {
                throw new Exception("Block Undo Failed");
            }
        }

        private async Task<bool> HandleChangeRequestAmount()
        {
            switch (OrderStatus)
            {
                case OrderStatus.OrderInBook:
                case OrderStatus.OrderCompletelyExecuted:
                    return await ReviseBlockedAmount();
                default:
                    if (RequestStatus != RequestStatus.ConfirmedByBourse)
                    {
                        return await ReviseBlockedAmount();
                    }

                    return true;
            }
        }

        private async Task<bool> HandleAmount()
        {
            switch (OrderStatus)
            {
                case OrderStatus.OrderInBook:
                case OrderStatus.OrderCompletelyExecuted:
                    return true;
                default:
                    return await ReleaseRemainingBlockedAmount();
            }
        }

        private async Task<bool> ReleaseRemainingBlockedAmount()
        {
            if (RemainingBlockedAmount != null)
            {
                return await HttpService.Post<bool>("http://localhost:5000/money/release", new
                {
                    CollaborationId,
                    Amount = RemainingBlockedAmount.Value
                });
            }

            return true;
        }

        private async Task<bool> ReviseBlockedAmount()
        {
            if (BlockedAmountChange != null)
            {
                return await HttpService.Post<bool>("http://localhost:5000/money/release", new
                {
                    CollaborationId,
                    Amount = BlockedAmountChange.Value
                });
            }

            return true;
        }
    }
}
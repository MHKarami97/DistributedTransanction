using Oms.Models.Enums;

namespace Oms.Models
{
    public class ConfirmationModel
    {
        public int RequestId { get; set; }
        public long? RemainingBlockedAmount { get; set; }
        public long? BlockedAmountChange { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
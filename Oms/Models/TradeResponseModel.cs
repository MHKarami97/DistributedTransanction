using Oms.Models.Enums;

namespace Oms.Models
{
    public sealed class TradeResponseModel
    {
        public long TradeNumber { get; set; }
        public DateTime TradeDate { get; set; }
        public Side OrderSide { get; set; }
        public long TradePrice { get; set; }
        public int TradeQuantity { get; set; }
        public string InstrumentName { get; set; }
        public int CustomerId { get; set; }
    }
}
using Oms.Models.Enums;

namespace Oms.Models
{
    public class Response
    {
        public int Id { get; set; }
        public long TradePrice { get; set; }
        public int CustomerId { get; set; }
        public int TradedQuantity { get; set; }
        public string InstrumentName { get; set; }
        public Side OrderSide { get; set; }
        public ResponseState ResponseState { get; set; }
    }
}
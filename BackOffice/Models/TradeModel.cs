using BackOffice.Models.Enums;

namespace BackOffice.Models
{
    public class TradeModel
    {
        public long TradePrice { get; set; }
        public int CustomerId { get; set; }
        public int TradedQuantity { get; set; }
        public string InstrumentName { get; set; }
        public Side OrderSide { get; set; }
        public int CollaborationId { get; set; }
    }
}
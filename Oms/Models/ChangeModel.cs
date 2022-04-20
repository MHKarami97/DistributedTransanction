namespace Oms.Models
{
    public class ChangeModel
    {
        public int RequestId { get; set; }  
        public string InstrumentName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int CustomerId { get; set; }
    }
}

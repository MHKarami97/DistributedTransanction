namespace Oms.Models;

public class Transaction
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public TransactionState TransactionState { get; set; }
}
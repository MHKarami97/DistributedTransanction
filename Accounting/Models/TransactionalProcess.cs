namespace Accounting.Models;

public class TransactionalProcess
{
    public int Id { get; set; }
    public int TracingCode { get; set; }
    public int BlockId { get; set; }
    public TransactionState TransactionState { get; set; }
}
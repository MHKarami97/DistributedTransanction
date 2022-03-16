namespace Saga;

public class TransactionContext<T>
{
    public T Context { get; set; }
    public int TracingCode { get; set; }
    public TransactionState State { get; set; }
}

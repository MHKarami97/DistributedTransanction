namespace Saga
{
    public interface IMicroTransaction<T>
    {
        Task Do(TransactionContext<T> context);
        Task Undo(TransactionContext<T> context);
    }
}

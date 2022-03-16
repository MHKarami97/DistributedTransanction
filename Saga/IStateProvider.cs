namespace Saga
{
    public interface IStateProvider<T>
    {
        TransactionContext<T> GetContext(int tracingId);
        void SaveContext(TransactionContext<T> context);
    }
}

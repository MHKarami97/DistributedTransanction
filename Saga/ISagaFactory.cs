namespace Saga
{
    public interface ISagaFactory
    {
        Saga<T> Create<T>(T initialState);
    }
}

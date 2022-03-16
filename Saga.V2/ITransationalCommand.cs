namespace Saga.V2
{
    public interface ITransationalCommand
    {
        Task Do();
        Task Undo();
    }
}

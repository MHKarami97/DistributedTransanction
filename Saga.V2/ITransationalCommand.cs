namespace Saga.V2
{
    public interface ITransationalCommand
    {
        int CollaborationId { get; set; }
        IServiceProvider ServiceProvider { get; set; }

        Task Do();
        Task Undo();
    }
}

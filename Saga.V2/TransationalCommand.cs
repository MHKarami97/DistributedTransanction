using System.Text.Json.Serialization;

namespace Saga.V2
{
    public abstract class TransationalCommand
    {
        public int CollaborationId { get; set; }

        [JsonIgnore]
        public IServiceProvider ServiceProvider { get; set; }

        public abstract Task Do();
        public abstract Task Undo();
    }
}

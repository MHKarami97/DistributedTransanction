using Saga.V2;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Oms.Repository
{
    public class DistributedTransactionModel
    {
        public DistributedTransactionModel(DistributedTransaction transaction)
        {
            CollaborationId = transaction.CollaborationId;
            CommandType = transaction.Command.GetType().FullName ?? "";
            CommandBody = JsonSerializer.Serialize(transaction.Command);
            State = transaction.State; 
        }

        public int Id { get; set; }
        public int CollaborationId { get; set; }
        public string CommandType { get; set; }
        public string CommandBody { get; set; }
        public TransactionState State { get; set; }
    }
}

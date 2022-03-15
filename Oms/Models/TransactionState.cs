namespace Oms.Models;

public enum TransactionState : byte
{
    Active = 0,
    Committed = 1,
    Aborted = 2,
    Indoubt = 3
}
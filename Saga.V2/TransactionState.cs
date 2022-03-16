namespace Saga.V2;

public enum TransactionState : byte
{
    Active = 1,
    Committed = 2,
    Aborted = 3,
    Failed = 4
}
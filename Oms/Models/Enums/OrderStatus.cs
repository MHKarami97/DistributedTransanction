namespace Oms.Models.Enums
{
    public enum OrderStatus
    {
        None = 0,
        OrderInBook = 1,
        OrderCancelledByBroker = 2,
        OrderCancelledBySurveillance = 3,
        OrderCancelledByCte = 4,
        OrderCancelledByInstrumentStateChange = 5,
        OrderCompletelyExecuted = 6,
        OrderRejectedByPotentialExecution = 7,
        OrderRejectedBySurveillance = 8,
        StopOrder = 9,
        OrderEliminated = 10
    }
}
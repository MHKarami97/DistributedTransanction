namespace Oms.Models.Enums
{
    public enum RequestStatus
    {
        SavedInAsa = 1,
        SendingToBourse = 2,
        SentToBourse = 3,
        ConfirmedByBourse = 4,
        QuitByError = 5
    }
}
namespace Oms.Models.Enums
{
    public enum ValidityType
    {
        None = 0,
        Day = 1,
        GoodTillDate = 2,
        GoodTillCancelled = 3,
        FillAndKill = 4,
        Session = 5,
        SlidingValidity = 6
    }
}
using System.Fabric.Chaos.DataStructures;

namespace ChaosTest;

internal class ChaosEventComparer : IEqualityComparer<ChaosEvent>
{
    public bool Equals(ChaosEvent? x, ChaosEvent? y)
    {
        return x != null && y != null && x.TimeStampUtc.Equals(y.TimeStampUtc);
    }
    public int GetHashCode(ChaosEvent obj)
    {
        return obj.TimeStampUtc.GetHashCode();
    }
}


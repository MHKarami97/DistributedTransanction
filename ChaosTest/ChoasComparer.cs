using System.Fabric.Chaos.DataStructures;

namespace ChaosTest;

class ChaosEventComparer : IEqualityComparer<ChaosEvent>
{
    public bool Equals(ChaosEvent x, ChaosEvent y)
    {
        return x.TimeStampUtc.Equals(y.TimeStampUtc);
    }
    public int GetHashCode(ChaosEvent obj)
    {
        return obj.TimeStampUtc.GetHashCode();
    }
}


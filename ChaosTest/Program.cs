using ChaosTest;
using System.Fabric;
using System.Fabric.Chaos.DataStructures;
using System.Fabric.Health;


string clusterConnectionString = "localhost:19000";
using (var client = new FabricClient(clusterConnectionString))
{
    var startTimeUtc = DateTime.UtcNow;

    // The maximum amount of time to wait for all cluster entities to become stable and healthy. 
    // Chaos executes in iterations and at the start of each iteration it validates the health of cluster
    // entities. 
    // During validation if a cluster entity is not stable and healthy within
    // MaxClusterStabilizationTimeoutInSeconds, Chaos generates a validation failed event.
    var maxClusterStabilizationTimeout = TimeSpan.FromSeconds(20.0);

    var timeToRun = TimeSpan.FromMinutes(10.0);

    // MaxConcurrentFaults is the maximum number of concurrent faults induced per iteration. 
    // Chaos executes in iterations and two consecutive iterations are separated by a validation phase.
    // The higher the concurrency, the more aggressive the injection of faults -- inducing more complex
    // series of states to uncover bugs.
    // The recommendation is to start with a value of 2 or 3 and to exercise caution while moving up.
    var maxConcurrentFaults = 100;

    // Describes a map, which is a collection of (string, string) type key-value pairs. The map can be
    // used to record information about the Chaos run. There cannot be more than 100 such pairs and
    // each string (key or value) can be at most 4095 characters long.
    // This map is set by the starter of the Chaos run to optionally store the context about the specific run.
    var startContext = new Dictionary<string, string> { { "ReasonForStart", "Testing" } };

    // Time-separation (in seconds) between two consecutive iterations of Chaos. The larger the value, the
    // lower the fault injection rate.
    var waitTimeBetweenIterations = TimeSpan.FromSeconds(1);

    // Wait time (in seconds) between consecutive faults within a single iteration.
    // The larger the value, the lower the overlapping between faults and the simpler the sequence of
    // state transitions that the cluster goes through. 
    // The recommendation is to start with a value between 1 and 5 and exercise caution while moving up.
    var waitTimeBetweenFaults = TimeSpan.Zero;

    // Passed-in cluster health policy is used to validate health of the cluster in between Chaos iterations. 
    var clusterHealthPolicy = new ClusterHealthPolicy
    {
        ConsiderWarningAsError = false,
        MaxPercentUnhealthyApplications = 100,
        MaxPercentUnhealthyNodes = 100
    };

    // All types of faults, restart node, restart code package, restart replica, move primary
    // replica, move secondary replica, and move instance will happen for nodes of type 'FrontEndType'
    var nodetypeInclusionList = new List<string> { "NodeType0" };

    // In addition to the faults included by nodetypeInclusionList,
    // restart code package, restart replica, move primary replica, move secondary replica,
    //  and move instance faults will happen for 'fabric:/TestApp2' even if a replica or code
    // package from 'fabric:/TestApp2' is residing on a node which is not of type included
    // in nodeypeInclusionList.
    var applicationInclusionList = new List<string> { "fabric:/Service" };

    // List of cluster entities to target for Chaos faults.
    var chaosTargetFilter = new ChaosTargetFilter
    {
        NodeTypeInclusionList = nodetypeInclusionList,
        ApplicationInclusionList = applicationInclusionList
    };

    var parameters = new ChaosParameters(
        maxClusterStabilizationTimeout,
        maxConcurrentFaults,
        false, /* EnableMoveReplicaFault */
        timeToRun,
        startContext,
        waitTimeBetweenIterations,
        waitTimeBetweenFaults,
        clusterHealthPolicy)
    { ChaosTargetFilter = chaosTargetFilter };

    await client.TestManager.StopChaosAsync();

    try
    {
        await client.TestManager.StartChaosAsync(parameters);
    }
    catch (FabricChaosAlreadyRunningException)
    {
        Console.WriteLine("An instance of Chaos is already running in the cluster.");
    }

    var filter = new ChaosReportFilter(startTimeUtc, DateTime.MaxValue);

    var eventSet = new HashSet<ChaosEvent>(new ChaosEventComparer());

    string continuationToken = null;

    while (true)
    {
        ChaosReport report;
        try
        {
            report = string.IsNullOrEmpty(continuationToken)
                ? await client.TestManager.GetChaosReportAsync(filter)
                : await client.TestManager.GetChaosReportAsync(continuationToken);
        }
        catch (Exception e)
        {
            if (e is FabricTransientException)
            {
                Console.WriteLine("A transient exception happened: '{0}'", e);
            }
            else if (e is TimeoutException)
            {
                Console.WriteLine("A timeout exception happened: '{0}'", e);
            }
            else
            {
                throw;
            }

            await Task.Delay(TimeSpan.FromSeconds(1.0));
            continue;
        }

        continuationToken = report.ContinuationToken;

        foreach (var chaosEvent in report.History)
        {
            if (eventSet.Add(chaosEvent))
            {
                Console.WriteLine(chaosEvent);
            }
        }

        // When Chaos stops, a StoppedEvent is created.
        // If a StoppedEvent is found, exit the loop.
        var lastEvent = report.History.LastOrDefault();

        if (lastEvent is StoppedEvent)
        {
            break;
        }

        await Task.Delay(TimeSpan.FromSeconds(1.0));
    }
}

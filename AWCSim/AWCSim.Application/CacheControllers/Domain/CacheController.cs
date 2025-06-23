using AWCSim.Application.CachesStatistics.Domain;
using AWCSim.Application.ReadPolicies.Abstract;
using AWCSim.Application.WritePolicies.Abstract;
using AWCSim.Core.Results;

namespace AWCSim.Application.CacheControllers.Domain;

public class CacheController
{
    protected ReadPolicy ReadPolicy { get; }
    protected WritePolicy WritePolicy { get; }
    protected Cache Cache { get; }
    public CacheStatistics Statistics => Cache.Statistics;

    internal CacheController(ReadPolicy readPolicy, WritePolicy writePolicy, Cache cache)
    {
        ReadPolicy = readPolicy;
        WritePolicy = writePolicy;
        Cache = cache;
    }

    public Result ExecuteRead(int address) => ReadPolicy.ExecuteRead(Cache, address);

    public Result ExecuteWrite(int address) => WritePolicy.ExecuteWrite(Cache, address);
}

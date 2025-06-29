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

    public Result ExecuteRead(int address)
    {
        ReadPolicy.ExecuteRead(Cache, address);
        Cache.Statistics.AddExecutedRead();

        return Result.Success();
    }


    public Result ExecuteWrite(int address)
    {
        WritePolicy.ExecuteWrite(Cache, address);
        Cache.Statistics.AddExecutedWrite();

        return Result.Success();
    }

    public void CleanCache()
    {
        var dirtyLinesQuantity = Cache.GetNumberOfDirtyLines();
        Statistics.AddMemoryWrites(dirtyLinesQuantity);
        Cache.Clear();
    }
}

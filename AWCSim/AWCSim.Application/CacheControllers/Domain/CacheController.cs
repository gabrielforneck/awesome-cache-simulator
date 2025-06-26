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
        var executionResult = ReadPolicy.ExecuteRead(Cache, address);
        if (executionResult.IsFailure)
            return executionResult;

        Cache.Statistics.AddExecutedRead();

        return Result.Success();
    }


    public Result ExecuteWrite(int address)
    {
        var executionResult = WritePolicy.ExecuteWrite(Cache, address);
        if (executionResult.IsFailure)
            return executionResult;

        Cache.Statistics.AddExecutedWrite();

        return Result.Success();
    }
}

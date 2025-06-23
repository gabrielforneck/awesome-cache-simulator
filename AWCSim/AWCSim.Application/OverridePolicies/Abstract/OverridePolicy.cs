using AWCSim.Application.CacheChunks.Domain;
using AWCSim.Application.CacheLines.Abstract;
using AWCSim.Application.CachesSpecifications.Domain;

namespace AWCSim.Application.OverridePolicies.Abstract;

public abstract class OverridePolicy
{
    protected CacheSpecifications CacheSpecifications { get; }

    protected OverridePolicy(CacheSpecifications cacheSpecifications)
    {
        CacheSpecifications = cacheSpecifications;
    }

    public abstract CacheLine PickLineToRemove(CacheChunk chunk);
}

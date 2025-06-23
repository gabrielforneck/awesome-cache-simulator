using AWCSim.Application.CacheChunks.Domain;
using AWCSim.Application.CacheLines.Abstract;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Abstract;

namespace AWCSim.Application.OverridePolicies.Implementations;

public class LeastRecentlyUsedOverridePolicy : OverridePolicy
{
    public LeastRecentlyUsedOverridePolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
    }

    public override CacheLine PickLineToRemove(CacheChunk chunk)
    {
        if (chunk.Length == 0)
            throw new InvalidOperationException("Chunk is empty.");

        var maxLastTimeUsed = chunk.Max(l => l.LastTimeUsed);
        return chunk.First(l => l.LastTimeUsed == maxLastTimeUsed);
    }
}

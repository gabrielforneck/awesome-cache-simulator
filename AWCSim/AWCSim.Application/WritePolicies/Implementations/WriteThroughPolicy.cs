using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.WritePolicies.Abstract;

namespace AWCSim.Application.WritePolicies.Implementations;

public class WriteThroughPolicy : WritePolicy
{
    public WriteThroughPolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
    }

    public override void ExecuteWrite(Cache cache, int address)
    {
        ExecuteCacheWriteIfExists(cache, address);
        cache.Statistics.AddMemoryWrite();
    }

    protected static void ExecuteCacheWriteIfExists(Cache cache, int address)
    {
        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
            return;

        var lineFound = chunkFound.FindLine(address);
        if (lineFound == null)
            return;

        cache.Statistics.AddCacheWrite();
        chunkFound.UpdateUses(lineFound);
    }
}

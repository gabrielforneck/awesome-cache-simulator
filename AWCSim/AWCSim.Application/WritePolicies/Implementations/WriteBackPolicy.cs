using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.WritePolicies.Abstract;

namespace AWCSim.Application.WritePolicies.Implementations;

public class WriteBackPolicy : WritePolicy
{
    public WriteBackPolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
    }

    public override void ExecuteWrite(Cache cache, int address)
    {
        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
        {
            cache.AddChunk(address, beginDirty: true);
            cache.Statistics.AddMemoryRead();
            return;
        }

        var lineFound = chunkFound.FindLine(address);
        if (lineFound != null)
        {
            lineFound.SetAsDirty();
            cache.Statistics.AddCacheWrite();
            chunkFound.UpdateUses(lineFound);
            return;
        }

        if (!chunkFound.IsFull)
        {
            chunkFound.AddLine(address, beginDirty: true);
            cache.Statistics.AddMemoryRead();
            return;
        }

        var selectedLine = cache.OverridePolicy.PickLineToRemove(chunkFound);
        chunkFound.OverrideLine(selectedLine, address, beginDirty: true);

        if (selectedLine.IsDirty)
            cache.Statistics.AddMemoryWrite();

        cache.Statistics.AddMemoryRead();
    }
}

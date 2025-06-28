using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.ReadPolicies.Abstract;

namespace AWCSim.Application.ReadPolicies.Implementations;

public class DefaultReadPolicy : ReadPolicy
{
    public override void ExecuteRead(Cache cache, int address)
    {
        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
        {
            cache.AddChunk(address);
            cache.Statistics.AddMemoryRead();
            return;
        }

        var lineFound = chunkFound.FindLine(address);
        if (lineFound != null)
        {
            cache.Statistics.AddCacheRead();
            chunkFound.UpdateUses(lineFound);
            return;
        }

        if (!chunkFound.IsFull)
        {
            chunkFound.AddLine(address);
            cache.Statistics.AddMemoryRead();
            return;
        }

        var selectedLine = cache.OverridePolicy.PickLineToRemove(chunkFound);
        chunkFound.OverrideLine(selectedLine, address);

        if (selectedLine.IsDirty)
            cache.Statistics.AddMemoryWrite();

        cache.Statistics.AddMemoryRead();
    }
}

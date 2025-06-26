using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.WritePolicies.Abstract;
using AWCSim.Core.Results;

namespace AWCSim.Application.WritePolicies.Implementations;

public class WriteBackPolicy : WritePolicy
{
    public WriteBackPolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
    }

    public override Result ExecuteWrite(Cache cache, int address)
    {
        if (!cache.Specifications.Address.AddressIsInRange(address))
            return Result.Failure($"O enderço {address:X} não está mapeado dentro da memória.");

        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
        {
            cache.AddChunk(address, beginDirty: true);
            cache.Statistics.AddMemoryRead();
            return Result.Success();
        }

        var lineFound = chunkFound.FindLine(address);
        if (lineFound != null)
        {
            lineFound.SetAsDirty();
            cache.Statistics.AddCacheWrite();
            chunkFound.UpdateUses(lineFound);
            return Result.Success();
        }

        if (!chunkFound.IsFull)
        {
            chunkFound.AddLine(address, beginDirty: true);
            cache.Statistics.AddMemoryRead();
            return Result.Success();
        }

        var selectedLine = cache.OverridePolicy.PickLineToRemove(chunkFound);
        chunkFound.OverrideLine(selectedLine, address, beginDirty: true);

        if (selectedLine.IsDirty)
            cache.Statistics.AddMemoryWrite();

        cache.Statistics.AddMemoryRead();

        return Result.Success();
    }
}

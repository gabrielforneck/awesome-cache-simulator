using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.WritePolicies.Abstract;
using AWCSim.Core.Results;

namespace AWCSim.Application.WritePolicies.Implementations;

public class WriteThroughPolicy : WritePolicy
{
    public WriteThroughPolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
    }

    public override Result ExecuteWrite(Cache cache, int address)
    {
        if (!cache.Specifications.Address.AddressIsInRange(address))
            return Result.Failure($"O enderço {address:X} não está mapeado dentro da memória.");

        ExecuteCacheWriteIfExists(cache, address); //TODO: Definir com o professor

        cache.Statistics.AddMemoryWrite();
        return Result.Success();
    }

    protected static void ExecuteCacheWriteIfExists(Cache cache, int address)
    {
        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
            return;

        var lineFound = chunkFound.FindLine(address);
        if (lineFound == null)
            return;

        cache.Statistics.AddCacheWrite(); //TODO: Definir com o professor
        chunkFound.UpdateUses(lineFound); //TODO: Definir com o professor
    }
}

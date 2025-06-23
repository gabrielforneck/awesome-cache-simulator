using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.ReadPolicies.Abstract;
using AWCSim.Core.Results;

namespace AWCSim.Application.ReadPolicies.Implementations;

public class DefaultReadPolicy : ReadPolicy
{
    public override Result ExecuteRead(Cache cache, int address)
    {
        if (!cache.Specifications.Address.AddressIsInRange(address))
            return Result.Failure($"O endereço {address} está fora do tamanho da cache.");

        var chunkFound = cache.FindChunk(address);
        if (chunkFound == null)
        {
            cache.AddChunk(address);
            cache.Statistics.AddMemoryRead();
            return Result.Success();
        }

        var lineFound = chunkFound.FindLine(address);
        if (lineFound != null)
        {
            cache.Statistics.AddCacheRead();
            chunkFound.UpdateUses(lineFound);
            return Result.Success();
        }

        if (!chunkFound.IsFull)
        {
            chunkFound.AddLine(address);
            cache.Statistics.AddMemoryRead();
            return Result.Success();
        }

        var selectedLine = cache.OverridePolicy.PickLineToRemove(chunkFound);
        chunkFound.OverrideLine(selectedLine, address);

        if (selectedLine.IsDirty)
            cache.Statistics.AddMemoryWrite();

        cache.Statistics.AddMemoryRead();
        return Result.Success();
    }
}

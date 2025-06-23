using AWCSim.Application.CacheChunks.Domain;
using AWCSim.Application.CacheLines.Abstract;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Abstract;

namespace AWCSim.Application.OverridePolicies.Implementations;

public class RandomOverridePolicy : OverridePolicy
{
    protected Random Random { get; }

    public RandomOverridePolicy(CacheSpecifications cacheSpecifications) : base(cacheSpecifications)
    {
        Random = new();
    }

    public override CacheLine PickLineToRemove(CacheChunk chunk)
    {
        if (chunk.Length == 0)
            throw new InvalidOperationException("Chunk is empty.");

        var randomIndex = Random.Next(0, chunk.Length);
        return chunk[randomIndex];
    }
}

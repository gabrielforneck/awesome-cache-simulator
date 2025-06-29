using AWCSim.Application.CacheChunks.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.CachesStatistics.Domain;
using AWCSim.Application.MainMemoriesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Abstract;

namespace AWCSim.Application.CacheControllers.Domain;

public class Cache
{
    public CacheSpecifications Specifications { get; }
    public CacheStatistics Statistics { get; }
    public OverridePolicy OverridePolicy { get; }
    protected List<CacheChunk> Chunks { get; }

    internal Cache(CacheSpecifications cacheSpecifications, MainMemorySpecifications mainMemorySpecifications, OverridePolicy overridePolicy)
    {
        Specifications = cacheSpecifications;
        Statistics = new(cacheSpecifications, mainMemorySpecifications);
        OverridePolicy = overridePolicy;
        Chunks = [];
    }

    public CacheChunk? FindChunk(int address) => Chunks.FirstOrDefault(c => c.AddressMatches(address));

    public void AddChunk(int address, bool beginDirty = false)
    {
        Chunks.Add(CacheChunk.Create(Specifications, address, beginDirty: beginDirty));
    }

    internal void Clear() => Chunks.Clear();

    internal int GetNumberOfDirtyLines() => Chunks.Sum(c => c.GetNumberOfDirtyLines());
}

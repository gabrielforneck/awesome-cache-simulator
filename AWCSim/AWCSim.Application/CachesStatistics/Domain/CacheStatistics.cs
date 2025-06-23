using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.MainMemoriesSpecifications.Domain;

namespace AWCSim.Application.CachesStatistics.Domain;

public class CacheStatistics
{
    public int TotalWrites => MainMemoryWrites + CacheWrites;
    public int TotalReads => MainMemoryReads + CacheReads;
    public int MainMemoryWrites { get; protected set; }
    public int MainMemoryReads { get; protected set; }
    public int CacheWrites { get; protected set; }
    public int CacheReads { get; protected set; }
    protected CacheSpecifications CacheSpecifications { get; }
    protected MainMemorySpecifications MainMemorySpecifications { get; }

    public CacheStatistics(CacheSpecifications cacheSpecifications, MainMemorySpecifications mainMemorySpecifications)
    {
        CacheSpecifications = cacheSpecifications;
        MainMemorySpecifications = mainMemorySpecifications;
    }

    public void AddMemoryWrite() => MainMemoryWrites++;

    public void AddMemoryRead() => MainMemoryReads++;

    public void AddCacheWrite() => CacheWrites++;

    public void AddCacheRead() => CacheReads++;

    public void WriteToFile(string fileName)
    {
        //TODO: Terminar
    }
}

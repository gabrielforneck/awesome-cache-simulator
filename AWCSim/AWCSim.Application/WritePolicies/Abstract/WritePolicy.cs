using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Core.Results;

namespace AWCSim.Application.WritePolicies.Abstract;

public abstract class WritePolicy
{
    protected CacheSpecifications CacheSpecifications { get; }

    protected WritePolicy(CacheSpecifications cacheSpecifications)
    {
        CacheSpecifications = cacheSpecifications;
    }

    public abstract void ExecuteWrite(Cache cache, int address);
}

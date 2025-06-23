using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Abstract;
using AWCSim.Application.OverridePolicies.Implementations;
using AWCSim.Application.OverridePolicies.Models.Enums;

namespace AWCSim.Application.OverridePolicies.Strategy;

public static class OverridePolicyStrategy
{
    public static OverridePolicy? Get(EOverridePolicy overridePolicy, CacheSpecifications cacheSpecifications) => overridePolicy switch
    {
        EOverridePolicy.LeastRecentlyUsed => new LeastRecentlyUsedOverridePolicy(cacheSpecifications),
        EOverridePolicy.Random => new RandomOverridePolicy(cacheSpecifications),
        _ => null
    };
}

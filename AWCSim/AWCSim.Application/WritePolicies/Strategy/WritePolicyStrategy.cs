using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.WritePolicies.Abstract;
using AWCSim.Application.WritePolicies.Implementations;
using AWCSim.Application.WritePolicies.Models.Enums;

namespace AWCSim.Application.WritePolicies.Strategy;

public static class WritePolicyStrategy
{
    public static WritePolicy? Get(EWritePolicy writePolicy, CacheSpecifications cacheSpecifications) => writePolicy switch
    {
        EWritePolicy.WriteBack => new WriteBackPolicy(cacheSpecifications),
        EWritePolicy.WriteThrough => new WriteThroughPolicy(cacheSpecifications),
        _ => null,
    };
}

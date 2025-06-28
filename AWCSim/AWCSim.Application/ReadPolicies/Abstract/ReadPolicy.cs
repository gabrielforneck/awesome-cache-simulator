using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.ReadPolicies.Implementations;

namespace AWCSim.Application.ReadPolicies.Abstract;

public abstract class ReadPolicy
{
    public static ReadPolicy Default => new DefaultReadPolicy();

    public abstract void ExecuteRead(Cache cache, int address);
}

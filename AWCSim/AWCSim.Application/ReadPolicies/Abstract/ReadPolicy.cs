using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.ReadPolicies.Implementations;
using AWCSim.Core.Results;

namespace AWCSim.Application.ReadPolicies.Abstract;

public abstract class ReadPolicy
{
    public static ReadPolicy Default => new DefaultReadPolicy();

    public abstract Result ExecuteRead(Cache cache, int address);
}

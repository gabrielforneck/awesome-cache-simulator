namespace AWCSim.Application.OverridePolicies.Models.Enums;

public enum EOverridePolicy
{
    LeastRecentlyUsed,
    Random
}

public static class EOverridePolicyExtensions
{
    public static string GetDescription(this EOverridePolicy policy) => policy switch
    {
        EOverridePolicy.LeastRecentlyUsed => "LRU",
        EOverridePolicy.Random => "Aleatória",
        _ => "Indefinido"
    };
}

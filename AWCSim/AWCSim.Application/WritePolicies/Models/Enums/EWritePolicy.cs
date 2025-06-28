namespace AWCSim.Application.WritePolicies.Models.Enums;

public enum EWritePolicy
{
    WriteThrough,
    WriteBack
}

public static class EWritePolicyExtensions
{
    public static string GetDescription(this EWritePolicy policy) => policy switch
    {
        EWritePolicy.WriteThrough => "Write-Through",
        EWritePolicy.WriteBack => "Write-Back",
        _ => "Indefinido"
    };
}

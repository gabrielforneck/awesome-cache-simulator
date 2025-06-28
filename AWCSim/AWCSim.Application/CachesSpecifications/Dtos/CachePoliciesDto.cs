using AWCSim.Application.OverridePolicies.Models.Enums;
using AWCSim.Application.WritePolicies.Models.Enums;

namespace AWCSim.Application.CachesSpecifications.Dtos;

public class CachePoliciesDto
{
    public EWritePolicy WritePolicy { get; }
    public EOverridePolicy OverridePolicy { get; }

    public CachePoliciesDto(EWritePolicy writePolicy, EOverridePolicy overridePolicy)
    {
        WritePolicy = writePolicy;
        OverridePolicy = overridePolicy;
    }
}

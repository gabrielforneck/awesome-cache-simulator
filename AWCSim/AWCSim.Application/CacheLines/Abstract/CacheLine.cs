using AWCSim.Application.CacheAddressesSpecifications.Domain;

namespace AWCSim.Application.CacheLines.Abstract;

public class CacheLine
{
    public int Tag { get; }
    public int LastTimeUsed { get; protected set; }
    public bool IsDirty { get; protected set; }
    protected CacheAddressSpecifications AddressSpecifications { get; }

    protected CacheLine(int tag, int lastTimeUsed, bool isDirty, CacheAddressSpecifications addressSpecifications)
    {
        Tag = tag;
        LastTimeUsed = lastTimeUsed;
        IsDirty = isDirty;
        AddressSpecifications = addressSpecifications;
    }

    public static CacheLine Create(int address, CacheAddressSpecifications addressSpecifications, bool isDirty = false)
    {
        var tag = addressSpecifications.GetTagFromAddress(address);
        return new(tag, 1, isDirty, addressSpecifications);
    }

    public bool AddressMatches(int address) => AddressSpecifications.GetTagFromAddress(address) == Tag;

    public void SetLastTimeUsed(int lastTimeUsed) => LastTimeUsed = lastTimeUsed;

    public void SetAsDirty() => IsDirty = true;
}

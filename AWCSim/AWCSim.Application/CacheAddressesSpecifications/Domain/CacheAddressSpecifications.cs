using AWCSim.Application.CachesSpecifications.Domain;

namespace AWCSim.Application.CacheAddressesSpecifications.Domain;

public class CacheAddressSpecifications
{
    public int AddressBits { get; }
    public int AddressMask { get; }
    public int TagBits { get; }
    public int TagMask { get; }
    public int IndexBits { get; }
    public int IndexMask { get; }
    public int OffsetBits { get; }
    public int OffsetMask { get; }

    private CacheAddressSpecifications(int addressBits, int addressMask, int tagBits, int tagMask, int indexBits, int indexMask, int offsetBits, int offsetMask)
    {
        AddressBits = addressBits;
        AddressMask = addressMask;
        TagBits = tagBits;
        TagMask = tagMask;
        IndexBits = indexBits;
        IndexMask = indexMask;
        OffsetBits = offsetBits;
        OffsetMask = offsetMask;
    }

    public static CacheAddressSpecifications Create(CacheSpecifications cacheSpecifications)
    {
        var offsetBits = (int)Math.Log(cacheSpecifications.LineSize, 2);
        var indexBits = (int)Math.Log(cacheSpecifications.LinesCount / cacheSpecifications.LinesPerChunkCount, 2);
        var addressBits = (int)Math.Log(cacheSpecifications.LinesCount * cacheSpecifications.LineSize, 2);
        var tagBits = addressBits - indexBits - offsetBits;

        var addressMask = CreateMask(addressBits);
        var tagMask = CreateMask(tagBits, indexBits + offsetBits);
        var indexMask = CreateMask(indexBits, offsetBits);
        var offsetMask = CreateMask(offsetBits);

        return new(addressBits, addressMask, tagBits, tagMask, indexBits, indexMask, offsetBits, offsetMask);
    }

    public bool AddressIsInRange(int address) => (address - address & AddressMask) == 0;

    public int GetTagFromAddress(int address) => address & TagMask;

    public int GetIndexFromAddress(int address) => address & IndexMask;

    public int GetOffsetFromAddress(int address) => address & OffsetMask;

    private static int CreateMask(int size, int offset) => CreateMask(size) << offset;

    private static int CreateMask(int size)
    {
        var mask = 0;

        for (int i = 0; i < size; i++)
            mask |= (1 << i);

        return mask;
    }
}

using AWCSim.Application.CachesSpecifications.Domain;

namespace AWCSim.Application.CacheAddressesSpecifications.Domain;

public class CacheAddressSpecifications
{
    public int TagBits { get; }
    public int TagMask { get; }
    public int IndexBits { get; }
    public int IndexMask { get; }
    public int OffsetBits { get; }
    public int OffsetMask { get; }

    protected const int AddressSize = 32;

    private CacheAddressSpecifications(int tagBits, int tagMask, int indexBits, int indexMask, int offsetBits, int offsetMask)
    {
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
        var tagBits = AddressSize - indexBits - offsetBits;

        var tagMask = CreateMask(tagBits, indexBits + offsetBits);
        var indexMask = CreateMask(indexBits, offsetBits);
        var offsetMask = CreateMask(offsetBits);

        return new(tagBits, tagMask, indexBits, indexMask, offsetBits, offsetMask);
    }

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

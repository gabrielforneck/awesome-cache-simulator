using AWCSim.Application.CacheAddressesSpecifications.Domain;
using AWCSim.Application.CacheLines.Abstract;
using AWCSim.Application.CachesSpecifications.Domain;
using System.Collections;

namespace AWCSim.Application.CacheChunks.Domain;

public class CacheChunk : IEnumerable<CacheLine>
{
    public int Index { get; set; }
    public bool IsFull => Lines.Count >= MaxLength;
    public int Length => Lines.Count;
    protected List<CacheLine> Lines { get; }
    protected int MaxLength { get; }
    protected CacheAddressSpecifications AddressSpecifications { get; }

    protected CacheChunk(int index, List<CacheLine> lines, int maxLength, CacheAddressSpecifications addressSpecifications)
    {
        Index = index;
        Lines = lines;
        MaxLength = maxLength;
        AddressSpecifications = addressSpecifications;
    }

    public static CacheChunk Create(CacheSpecifications cacheSpecifications, int address, bool beginDirty = false)
    {
        var index = cacheSpecifications.Address.GetIndexFromAddress(address);
        var maxLength = cacheSpecifications.LinesPerChunkCount;
        var line = CacheLine.Create(address, cacheSpecifications.Address, isDirty: beginDirty);

        return new(index, [line], maxLength, cacheSpecifications.Address);
    }

    public IEnumerator<CacheLine> GetEnumerator() => Lines.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public CacheLine this[int index] => Lines[index];

    public bool AddressMatches(int address) => AddressSpecifications.GetIndexFromAddress(address) == Index;

    public CacheLine? FindLine(int address) => Lines.FirstOrDefault(l => l.AddressMatches(address));

    public void AddLine(int address, bool beginDirty = false)
    {
        if (!AddressMatches(address))
            throw new InvalidOperationException("The address index does not matches the chunk index.");

        if (IsFull)
            throw new InvalidOperationException("The chunk is full.");

        var newLine = CacheLine.Create(address, AddressSpecifications, isDirty: beginDirty);
        AddLine(newLine);
    }

    public void OverrideLine(CacheLine line, int address, bool beginDirty = false)
    {
        if (!AddressMatches(address))
            throw new InvalidOperationException("The address index does not matches the chunk index.");

        if (!Lines.Remove(line))
            throw new InvalidOperationException("The line is not contained inside this chunk.");

        var newLine = CacheLine.Create(address, AddressSpecifications, isDirty: beginDirty);
        AddLine(newLine);
    }

    protected void AddLine(CacheLine line)
    {
        Lines.Add(line);
        UpdateUses(line);
    }

    public void UpdateUses(CacheLine lastTimeUsedLine)
    {
        var orderedLines = Lines.OrderBy(l => l.Tag != lastTimeUsedLine.Tag).ThenBy(l => l.LastTimeUsed);

        var lastTimeUsed = 0;

        foreach (var line in orderedLines)
            line.SetLastTimeUsed(++lastTimeUsed);
    }

    public int GetNumberOfDirtyLines() => Lines.Count(l => l.IsDirty);
}

namespace AWCSim.Application.Simulation.Commands;

public abstract class CacheCommand
{
    public int Address { get; }

    protected CacheCommand(int address)
    {
        Address = address;
    }

    public static CacheCommand Read(int address) => new ReadCommand(address);
    public static CacheCommand Write(int address) => new WriteCommand(address);
}

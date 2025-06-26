using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.MainMemoriesSpecifications.Domain;
using AWCSim.Core.TextReport;

namespace AWCSim.Application.CachesStatistics.Domain;

public class CacheStatistics
{
    public int TotalExecutedWrites { get; protected set; }
    public int TotalExecutedReads { get; protected set; }
    public int TotalExecutions => TotalExecutedWrites + TotalExecutedReads;
    public int MainMemoryWrites { get; protected set; }
    public int MainMemoryReads { get; protected set; }
    public int TotalMainMemoryHits => MainMemoryWrites + MainMemoryReads;
    public int CacheWrites { get; protected set; }
    public int CacheReads { get; protected set; }
    public int TotalCacheHits => CacheWrites + CacheReads;
    protected CacheSpecifications CacheSpecifications { get; }
    protected MainMemorySpecifications MainMemorySpecifications { get; }

    public CacheStatistics(CacheSpecifications cacheSpecifications, MainMemorySpecifications mainMemorySpecifications)
    {
        CacheSpecifications = cacheSpecifications;
        MainMemorySpecifications = mainMemorySpecifications;
    }

    public void AddExecutedWrite() => TotalExecutedWrites++;

    public void AddExecutedRead() => TotalExecutedReads++;

    public void AddMemoryWrite() => MainMemoryWrites++;

    public void AddMemoryRead() => MainMemoryReads++;

    public void AddCacheWrite() => CacheWrites++;

    public void AddCacheRead() => CacheReads++;

    public void WriteToFile() => WriteToFile(Program.Configuration.ReportOutputFileLocation);

    public void WriteToFile(string path)
        => new TextReportWriter(path)
        .AddLine("Especificações da memória cache:")
        .AddLine($"Tamanho da linha: {CacheSpecifications.LineSize}")
        .AddLine($"Quantidade de linhas: {CacheSpecifications.LinesCount}")
        .AddLine($"Linhas por conjunto: {CacheSpecifications.LinesPerChunkCount}")
        .AddLine($"Tempo de operação: {CacheSpecifications.SuccessfulOperationTime}ns")
        .AddSection()
        .AddLine("Especificações da memória principal:")
        .AddLine($"Tempo de operação: {MainMemorySpecifications.OperationTime}ns")
        .AddSection()
        .AddLine("Resultados da execução:")
        .AddLine($"Total de leituras e escritas realizadas: {TotalExecutions}")
        .AddLine($"Total de leituras e escritas realizadas na memória principal: {TotalMainMemoryHits}")
        .AddLine($"Taxa de acerto de leituras: {Math.Round(CacheReads / (TotalExecutedReads * 1.0) * 100, 4)}% ({CacheReads})")
        .AddLine($"Taxa de acerto de escrita: {Math.Round(CacheWrites / (TotalExecutedWrites * 1.0) * 100, 4)}% ({CacheWrites})")
        .AddLine($"Taxa de acerto (leituras + escritas): {Math.Round(TotalCacheHits / (TotalExecutions * 1.0) * 100, 4)}% ({TotalCacheHits})")
        .AddLine($"Tempo médio de acesso: {Math.Round(ComputeAverageAccessTime(), 4)}ns");

    public double ComputeAverageAccessTime()
    {
        var hitRate = TotalCacheHits / (TotalExecutions * 1.0);
        return CacheSpecifications.SuccessfulOperationTime + (1 - hitRate) * MainMemorySpecifications.OperationTime;
    }
}

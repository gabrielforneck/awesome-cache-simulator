using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.MainMemoriesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Models.Enums;
using AWCSim.Application.WritePolicies.Models.Enums;
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

    public void WriteToFile() => WriteToFile(GetOutputFileLocation());

    public void WriteToFile(string path)
        => new TextReportWriter("RESULTADOS DA EXECUÇÃO")
        .AddLine("Especificações da memória cache:")
        .AddLine("Tamanho da linha: {0}", CacheSpecifications.LineSize)
        .AddLine("Quantidade de linhas: {0}", CacheSpecifications.LinesCount)
        .AddLine("Linhas por conjunto: {0}", CacheSpecifications.LinesPerChunkCount)
        .AddLine("Tempo de operação: {0}ns", CacheSpecifications.SuccessfulOperationTime)
        .AddLine("Política de escrita: {0}", CacheSpecifications.Policies.WritePolicy.GetDescription())
        .AddLine("Política de sobrescrita: {0}", CacheSpecifications.Policies.OverridePolicy.GetDescription())
        .AddSection()
        .AddLine("Especificações da memória principal:")
        .AddLine("Tempo de operação: {0}ns", MainMemorySpecifications.OperationTime)
        .AddSection()
        .AddLine("Resultados da execução:")
        .AddLine("Total de leituras realizadas: {0}", TotalExecutedReads)
        .AddLine("Total de escritas realizadas: {0}", TotalExecutedWrites)
        .AddLine("Total de leituras e escritas realizadas: {0}", TotalExecutions)
        .AddLine("Total de leituras realizadas na memória principal: {0}", MainMemoryReads)
        .AddLine("Total de escritas realizadas na memória principal: {0}", MainMemoryWrites)
        .AddLine("Total de leituras e escritas realizadas na memória principal: {0}", TotalMainMemoryHits)
        .AddLine("Taxa de acerto de leituras: {0:0.0000}% ({1})", CacheReads / (TotalExecutedReads * 1.0) * 100, CacheReads)
        .AddLine("Taxa de acerto de escrita: {0:0.0000}% ({1})", CacheWrites / (TotalExecutedWrites * 1.0) * 100, CacheWrites)
        .AddLine("Taxa de acerto (leituras + escritas): {0:0.0000}% ({1})", TotalCacheHits / (TotalExecutions * 1.0) * 100, TotalCacheHits)
        .AddLine("Tempo médio de acesso: {0:0.0000}ns", ComputeAverageAccessTime())
        .Write(path);

    public double ComputeAverageAccessTime()
    {
        var hitRate = TotalCacheHits / (TotalExecutions * 1.0);
        return CacheSpecifications.SuccessfulOperationTime + (1 - hitRate) * MainMemorySpecifications.OperationTime;
    }

    protected string GetOutputFileLocation()
    {
        var path = Program.Configuration.ReportOutputFileLocation;

        if (string.IsNullOrWhiteSpace(path))
            return GenerateFileName();

        return Path.Combine(path, GenerateFileName());
    }

    protected string GenerateFileName()
        => string.Format("report_{0}_{1}_{2}_{3}_{4}_{5}_{6}.txt",
            CacheSpecifications.LineSize,
            CacheSpecifications.LinesCount,
            CacheSpecifications.LinesPerChunkCount,
            CacheSpecifications.SuccessfulOperationTime,
            MainMemorySpecifications.OperationTime,
            CacheSpecifications.Policies.WritePolicy,
            CacheSpecifications.Policies.OverridePolicy);
}

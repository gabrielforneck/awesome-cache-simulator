using AWCSim.Application.OverridePolicies.Models.Enums;
using AWCSim.Application.WritePolicies.Models.Enums;
using AWCSim.Core.Results;
using System.Text.Json;

namespace AWCSim.Application.Configuration;

public class AppConfiguration
{
    public EWritePolicy WritePolicy { get; set; }
    public int LineSize { get; set; }
    public int LinesCount { get; set; }
    public int LinesPerChunkCount { get; set; }
    public int CacheOperationTime { get; set; }
    public int MainMemoryOperationTime { get; set; }
    public EOverridePolicy OverridePolicy { get; set; }
    public string? ReportOutputFileLocation { get; set; }
    public string MemoryCommandsFileLocation { get; set; }

    protected const string DefaultFileLocation = "appsettings.json";

    public AppConfiguration(EWritePolicy writePolicy, int lineSize, int linesCount, int linesPerChunkCount, int cacheOperationTime, int mainMemoryOperationTime, EOverridePolicy overridePolicy, string reportOutputFileLocation, string memoryCommandsFileLocation)
    {
        WritePolicy = writePolicy;
        LineSize = lineSize;
        LinesCount = linesCount;
        LinesPerChunkCount = linesPerChunkCount;
        CacheOperationTime = cacheOperationTime;
        MainMemoryOperationTime = mainMemoryOperationTime;
        OverridePolicy = overridePolicy;
        ReportOutputFileLocation = reportOutputFileLocation;
        MemoryCommandsFileLocation = memoryCommandsFileLocation;
    }

    public static Result<AppConfiguration> LoadFromDefaultFileLocation()
    {
        try
        {
            var fileContent = File.ReadAllText(DefaultFileLocation);
            var appConfiguration = JsonSerializer.Deserialize<AppConfiguration>(fileContent);

            if (appConfiguration == null)
                return Result.Failure<AppConfiguration>("Não foi possível converter o arquivo de configurações.");

            var validationResult = appConfiguration.Validate();
            if (validationResult.IsFailure)
                return Result.Failure<AppConfiguration>(validationResult.Error);

            return Result.Success(appConfiguration);
        }
        catch (FileNotFoundException)
        {
            return Result.Failure<AppConfiguration>("Arquivo de configurações não encontrado.");
        }
        catch (Exception e)
        {
            return Result.Failure<AppConfiguration>($"Ocorreu um erro ao carregar as configurações do programa: {e.Message}.");
        }
    }

    public Result Validate()
    {
        if (string.IsNullOrWhiteSpace(MemoryCommandsFileLocation))
            return Result.Failure("Deve ser especificado o local do arquivo dos endereços da cache.");

        return Result.Success();
    }
}

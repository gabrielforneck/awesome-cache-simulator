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
    public string ReportOutputFileLocation { get; set; } = string.Empty;
    public string MemoryCommandsFileLocation { get; set; } = string.Empty;

    protected const string DefaultFileLocation = "appsettings.json";

    protected AppConfiguration() { }

    public static Result<AppConfiguration> LoadFromDefaultFileLocation()
    {
        try
        {
            var fileContent = File.ReadAllText(DefaultFileLocation);
            var appConfiguration = JsonSerializer.Deserialize<AppConfiguration>(fileContent);

            if (appConfiguration == null)
                return Result.Failure<AppConfiguration>("Não foi possível converter o arquivo de configurações.");

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
}

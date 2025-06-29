using AWCSim.Application.CacheControllers.Builders;
using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesStatistics.Domain;
using AWCSim.Application.Simulation.Commands;
using AWCSim.Application.Simulation.CommandsProvirders;
using AWCSim.Core.Results;

namespace AWCSim.Application.Simulation.Handlers;

public static class SimulationHandler
{
    public static Result<CacheStatistics> Run()
    {
        var createCommandProviderResult = CommandProvider.Create(Program.Configuration.MemoryCommandsFileLocation);
        if (createCommandProviderResult.IsFailure)
            return Result.Failure<CacheStatistics>(createCommandProviderResult.Error);

        using var commandProvider = createCommandProviderResult.Value;

        var controllerBuildResult = BuildCacheController();
        if (controllerBuildResult.IsFailure)
            return Result.Failure<CacheStatistics>(controllerBuildResult.Error);

        return Run(commandProvider, controllerBuildResult.Value);
    }

    private static Result<CacheStatistics> Run(CommandProvider commandProvider, CacheController cacheController)
    {
        while (true)
        {
            var getResult = commandProvider.GetNextCommand();
            if (getResult.IsFailure)
                return Result.Failure<CacheStatistics>(getResult.Error);

            if (getResult.Value == null)
                break;

            var executionResult = ExecuteOperation(getResult.Value, cacheController);
            if (executionResult.IsFailure)
                return Result.Failure<CacheStatistics>(executionResult.Error);
        }

        cacheController.CleanCache();

        return Result.Success(cacheController.Statistics);
    }

    private static Result ExecuteOperation(CacheCommand command, CacheController cacheController)
    {
        if (command is ReadCommand readCommand)
            return cacheController.ExecuteRead(readCommand.Address);

        if (command is WriteCommand writeCommand)
            return cacheController.ExecuteWrite(writeCommand.Address);

        return Result.Failure("Operação inesperada na cache.");
    }

    private static Result<CacheController> BuildCacheController()
        => new CacheControllerBuilder()
            .WithWritePolicy(Program.Configuration.WritePolicy)
            .WithLineSize(Program.Configuration.LineSize)
            .WithLinesCount(Program.Configuration.LinesCount)
            .WithLinesPerChunkCount(Program.Configuration.LinesPerChunkCount)
            .WithCacheOperationTime(Program.Configuration.CacheOperationTime)
            .WithMainMemoryOperationTime(Program.Configuration.MainMemoryOperationTime)
            .WithOverridePolicy(Program.Configuration.OverridePolicy)
            .Build();
}

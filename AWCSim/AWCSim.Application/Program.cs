﻿using AWCSim.Application.Configuration;
using AWCSim.Application.Simulation.Handlers;

namespace AWCSim.Application;

public static class Program
{
    public static AppConfiguration Configuration => ProgramConfiguration ?? throw new InvalidOperationException("Configuration not loaded yet.");
    private static AppConfiguration? ProgramConfiguration { get; set; }

    public static void Main()
    {
        var loadAppConfigurationResult = AppConfiguration.Load();
        if (loadAppConfigurationResult.IsFailure)
            Terminate(loadAppConfigurationResult.Error, -1);

        ProgramConfiguration = loadAppConfigurationResult.Value;

        var runResult = SimulationHandler.Run();
        if (runResult.IsFailure)
            Terminate(runResult.Error, -1);

        runResult.Value.WriteToFile();
    }

    private static void Terminate(string errorMessage, int exitCode)
    {
        Console.WriteLine(errorMessage);
        Environment.Exit(exitCode);
    }
}
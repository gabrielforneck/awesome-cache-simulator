using AWCSim.Application.Simulation.Commands;
using AWCSim.Core.Results;
using System.Diagnostics.CodeAnalysis;

namespace AWCSim.Application.Simulation.CommandsProvirders;

public class CommandProvider : IDisposable
{
    protected FileStream Stream { get; }
    protected Lazy<StreamReader> Reader { get; }
    protected bool Disposed { get; set; }

    protected CommandProvider(FileStream stream)
    {
        Stream = stream;
        Reader = new(() => new StreamReader(Stream, leaveOpen: true));
        Disposed = false;
    }

    public static Result<CommandProvider> Create(string fileLocation)
    {
        try
        {
            var fileStream = File.OpenRead(fileLocation);
            return Result.Success(new CommandProvider(fileStream));
        }
        catch (FileNotFoundException)
        {
            return Result.Failure<CommandProvider>("Arquivo de comandos da memória não encontrado.");
        }
        catch (DirectoryNotFoundException)
        {
            return Result.Failure<CommandProvider>("Arquivo de comandos da memória não encontrado.");
        }
        catch (Exception ex)
        {
            return Result.Failure<CommandProvider>($"Não foi possível abrir o arquivo de comandos: {ex.Message}.");
        }
    }

    public Result<CacheCommand?> GetNextCommand()
    {
        ObjectDisposedException.ThrowIf(Disposed, this);

        if (!GetNextLine(out var lineRead))
            return Result.Success<CacheCommand?>(null);

        var parseResult = ParseLine(lineRead);
        if (parseResult.IsFailure)
            return Result.Failure<CacheCommand?>(parseResult.Error);

        return Result.Success<CacheCommand?>(parseResult.Value);
    }

    protected bool GetNextLine([NotNullWhen(true)] out string? line)
    {
        while (!Reader.Value.EndOfStream)
        {
            line = Reader.Value.ReadLine();
            if (line != null && line.Length > 0)
                return true;
        }

        line = null;
        return false;
    }

    protected static Result<CacheCommand> ParseLine(string line)
    {
        var lineParts = line.Split(' ');
        if (lineParts.Length != 2)
            return Result.Failure<CacheCommand>($"A linha '{line}' não está no formato esperado.");

        if (!TryParseAddress(lineParts.First(), out var address))
            return Result.Failure<CacheCommand>($"O endereço da linha '{line}' não pode ser convertido.");

        if (lineParts.Last().ToLower().Equals("r"))
            return Result.Success(CacheCommand.Read(address.Value));

        if (lineParts.Last().ToLower().Equals("w"))
            return Result.Success(CacheCommand.Write(address.Value));

        return Result.Failure<CacheCommand>($"Comando da linha '{line}' não mapeado.");
    }

    protected static bool TryParseAddress(string address, [NotNullWhen(true)] out int? parsedAddress)
    {
        parsedAddress = null;

        try
        {
            parsedAddress = Convert.ToInt32(address, 16);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || Disposed)
            return;

        if (Reader.IsValueCreated)
            Reader.Value.Dispose();

        Stream.Dispose();
    }
}

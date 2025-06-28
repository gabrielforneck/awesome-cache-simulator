using AWCSim.Application.CacheControllers.Domain;
using AWCSim.Application.CachesSpecifications.Domain;
using AWCSim.Application.MainMemoriesSpecifications.Domain;
using AWCSim.Application.OverridePolicies.Models.Enums;
using AWCSim.Application.OverridePolicies.Strategy;
using AWCSim.Application.ReadPolicies.Abstract;
using AWCSim.Application.WritePolicies.Models.Enums;
using AWCSim.Application.WritePolicies.Strategy;
using AWCSim.Core.Results;

namespace AWCSim.Application.CacheControllers.Builders;

public class CacheControllerBuilder
{
    public EWritePolicy WritePolicy { get; private set; }
    public int LineSize { get; private set; }
    public int LinesCount { get; private set; }
    public int LinesPerChunkCount { get; private set; }
    public int CacheOperationTime { get; private set; }
    public int MainMemoryOperationTime { get; private set; }
    public EOverridePolicy OverridePolicy { get; private set; }

    public CacheControllerBuilder() { }

    public CacheControllerBuilder WithLineSize(int lineSize)
    {
        LineSize = lineSize;
        return this;
    }

    public CacheControllerBuilder WithLinesCount(int linesCount)
    {
        LinesCount = linesCount;
        return this;
    }

    public CacheControllerBuilder WithLinesPerChunkCount(int linesPerChunkCount)
    {
        LinesPerChunkCount = linesPerChunkCount;
        return this;
    }

    public CacheControllerBuilder WithWritePolicy(EWritePolicy writePolicy)
    {
        WritePolicy = writePolicy;
        return this;
    }

    public CacheControllerBuilder WithOverridePolicy(EOverridePolicy overridePolicy)
    {
        OverridePolicy = overridePolicy;
        return this;
    }

    public CacheControllerBuilder WithCacheOperationTime(int cacheOperationTime)
    {
        CacheOperationTime = cacheOperationTime;
        return this;
    }

    public CacheControllerBuilder WithMainMemoryOperationTime(int mainMemoryOperationTime)
    {
        MainMemoryOperationTime = mainMemoryOperationTime;
        return this;
    }

    public Result<CacheController> Build()
    {
        var cacheSpecifications = new CacheSpecifications(LineSize, LinesCount, LinesPerChunkCount, CacheOperationTime, WritePolicy, OverridePolicy);
        var cacheSpecificationsValidationResult = cacheSpecifications.Validate();

        if (cacheSpecificationsValidationResult.IsFailure)
            return Result.Failure<CacheController>(cacheSpecificationsValidationResult.Error);

        var writePolicy = WritePolicyStrategy.Get(WritePolicy, cacheSpecifications);
        if (writePolicy == null)
            return Result.Failure<CacheController>("Política de escrita inválida.");

        var overridePolicy = OverridePolicyStrategy.Get(OverridePolicy, cacheSpecifications);
        if (overridePolicy == null)
            return Result.Failure<CacheController>("Política de substituição inválida.");

        var mainMemorySpecifications = new MainMemorySpecifications(MainMemoryOperationTime);
        var mainMemorySpecificationsValidationResult = mainMemorySpecifications.Validate();

        if (mainMemorySpecificationsValidationResult.IsFailure)
            return Result.Failure<CacheController>(mainMemorySpecificationsValidationResult.Error);

        var cache = new Cache(cacheSpecifications, mainMemorySpecifications, overridePolicy);

        return Result.Success(new CacheController(ReadPolicy.Default, writePolicy, cache));
    }
}

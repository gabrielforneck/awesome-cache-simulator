﻿using AWCSim.Application.CacheAddressesSpecifications.Domain;
using AWCSim.Application.CachesSpecifications.Dtos;
using AWCSim.Application.OverridePolicies.Models.Enums;
using AWCSim.Application.WritePolicies.Models.Enums;
using AWCSim.Core.Extensions.Maths;
using AWCSim.Core.Results;

namespace AWCSim.Application.CachesSpecifications.Domain;

public class CacheSpecifications
{
    public int LineSize { get; }
    public int LinesCount { get; }
    public int LinesPerChunkCount { get; }
    public int SuccessfulOperationTime { get; }
    public CachePoliciesDto Policies { get; }
    public CacheAddressSpecifications Address { get; }

    public CacheSpecifications(int lineSize, int linesCount, int linesPerChunkCount, int successfulOperationTime, EWritePolicy writePolicy, EOverridePolicy overridePolicy)
    {
        LineSize = lineSize;
        LinesCount = linesCount;
        LinesPerChunkCount = linesPerChunkCount;
        SuccessfulOperationTime = successfulOperationTime;
        Policies = new(writePolicy, overridePolicy);
        Address = CacheAddressSpecifications.Create(this);
    }

    public Result Validate()
    {
        if (LineSize <= 0)
            return Result.Failure("O tamanho da linha deve ser maior que 0.");

        if (!MathExtensions.IsPowerOf(LineSize, 2))
            return Result.Failure("O tamanho da linha deve ser uma potência de 2.");

        if (LinesCount <= 0)
            return Result.Failure("A quantidade de linhas deve ser maior que 0.");

        if (!MathExtensions.IsPowerOf(LinesCount, 2))
            return Result.Failure("A quantidade de linhas deve ser uma potência de 2.");

        if (LinesPerChunkCount <= 0)
            return Result.Failure("A quantidade de linhas por conjunto deve ser maior que 0.");

        if (!MathExtensions.IsPowerOf(LinesPerChunkCount, 2))
            return Result.Failure("A quantidade de linhas por conjunto deve ser uma potência de 2.");

        if (LinesPerChunkCount > LinesCount)
            return Result.Failure("A quantidade de linhas por conjunto deve ser menor ou igual a quantidade de linhas na cache.");

        if (LinesCount % LinesPerChunkCount != 0)
            return Result.Failure("A quantidade de linhas da cache deve ser divisível pela quantidade de linhas por conjunto.");

        if (SuccessfulOperationTime <= 0)
            return Result.Failure("O tempo de operações da cache deve ser maior que 0.");

        return Result.Success();
    }
}

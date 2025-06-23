using AWCSim.Core.Results;

namespace AWCSim.Application.MainMemoriesSpecifications.Domain;

public class MainMemorySpecifications
{
    public int OperationTime { get; }

    public MainMemorySpecifications(int operationTime)
    {
        OperationTime = operationTime;
    }

    public Result Validate()
    {
        if (OperationTime <= 0)
            return Result.Failure("Tempo de operação com a memória principal deve ser maior que 0.");

        return Result.Success();
    }
}

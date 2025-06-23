namespace AWCSim.Core.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    protected string ErrorMessage { get; }
    public string Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot read error when result is success.");

            return ErrorMessage;
        }
    }

    internal Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new Result(true, string.Empty);
    public static Result Failure(string error) => new Result(false, error);
    public static Result<T> Success<T>(T value) => new Result<T>(true, string.Empty, value);
    public static Result<T> Failure<T>(string error) => new Result<T>(true, error, default);
}

public class Result<T> : Result
{
    protected T? ResultValue { get; }
    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot read value when result is failure.");

            return ResultValue!;
        }
    }

    internal Result(bool isSuccess, string errorMessage, T? value) : base(isSuccess, errorMessage)
    {
        ResultValue = value;
    }
}

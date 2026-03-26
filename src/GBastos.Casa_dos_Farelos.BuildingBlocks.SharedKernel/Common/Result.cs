namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;

public readonly struct Result<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? Error { get; }

    private Result(bool success, T? data, string? error)
    {
        Success = success;
        Data = data;
        Error = error;
    }

    public static Result<T> Ok(T data)
        => new Result<T>(true, data, null);

    public static Result<T> Fail(string error)
        => new Result<T>(false, default, error);

    public static Result<Guid> OkGuid(Guid id)
        => Result<Guid>.Ok(id);
}
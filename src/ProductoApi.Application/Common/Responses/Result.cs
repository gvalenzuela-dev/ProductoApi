
namespace ProductoApi.Application.Common.Responses;

public class Result<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public List<string>? Errors { get; }

    private Result(T? data)
    {
        Success = true;
        Data = data ?? default;
    }
    private Result(List<string> errors)
    {
        Success = false;
        Errors = errors ?? [];
    }

    public static Result<T> Ok(T data) => new(data);
    public static Result<T> Fail(string error) => new([error]);
    public static Result<T> Fail(List<string> errors) => new(errors);
}
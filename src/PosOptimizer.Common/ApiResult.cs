using PosOptimizer.Common.Enums;
using PosOptimizer.Common.Extensions;

namespace PosOptimizer.Common;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ErrorCode? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    private ApiResult() {}

    #region Success
    
    public static ApiResult<T> Ok(T data)
    {
        return new ApiResult<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ApiResult<T> Ok()
    {
        return new ApiResult<T>
        {
            Success = true
        };
    }

    #endregion

    #region Fail

    public static ApiResult<T> Fail(ErrorCode code, string? customMessage = null)
    {
        return new ApiResult<T>
        {
            Success = false,
            ErrorCode = code,
            ErrorMessage = customMessage ?? code.GetDescription()
        };
    }

    #endregion
}

namespace OFFICEKIT_CORE_ATTENDANCE.Results
{
    namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO
    {
        public class Result<T>
        {
            public bool IsSuccess { get; }
            public T? Data { get; }
            public string? Error { get; }
            public int StatusCode { get; }

            // Private constructor to enforce usage of factory methods
            private Result(bool isSuccess, T? data, string? error, int statusCode)
            {
                IsSuccess = isSuccess;
                Data = data;
                Error = error;
                StatusCode = statusCode;
            }

            // Factory method for success
            public static Result<T> Success(T data, int statusCode = 200)
            {
                return new Result<T>(true, data, null, statusCode);
            }

            // Factory method for failure
            public static Result<T> Failure(string error, int statusCode = 400)
            {
                return new Result<T>(false, default, error, statusCode);
            }

            // Factory method for validation errors
            public static Result<T> ValidationError(string error, int statusCode = 422)
            {
                return new Result<T>(false, default, error, statusCode);
            }
        }
    }
}

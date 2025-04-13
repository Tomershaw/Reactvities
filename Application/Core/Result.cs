using MediatR;

namespace Application.Core
{
    // Represents the result of an operation, encapsulating success status, value, and error message.
    // This class is used to standardize the way results are returned from application logic.

    public class Result<T>
    {
        // Indicates whether the operation was successful.
        public bool IsSuccess { get; set; }

        // The value returned by the operation if it was successful.
        public T Value { get; set; }

        // The error message describing why the operation failed, if applicable.
        public string Error { get; set; }

        // Factory method to create a successful result with the specified value.
        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

        // Factory method to create a failed result with the specified error message.
        public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
    }
}

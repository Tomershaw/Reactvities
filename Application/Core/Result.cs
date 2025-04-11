using MediatR;

namespace Application.Core
{
    // Generic wrapper class for handling operation results consistently.
    // Used in MediatR request handlers to return either a successful result or an error.

    public class Result<T>
    {
        // Indicates whether the operation succeeded
        public bool IsSuccess { get; set; }

        // The result value (if successful)
        public T Value { get; set; }

        // The error message (if failed)
        public string Error { get; set; }

        // Factory method for creating a successful result
        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

        // Factory method for creating a failed result
        public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
    }
}

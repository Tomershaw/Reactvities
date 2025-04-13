namespace Application.Core
{
    // Represents a standardized structure for API error responses.
    // Used by global exception middleware to return consistent error details to the client.

    public class AppException
    {
        // Constructor to initialize the exception with status code, message, and optional details.
        public AppException(int statusCode, string message, string detalis = null)
        {
            StatusCode = statusCode; // The HTTP status code (e.g., 400 for Bad Request, 500 for Internal Server Error).
            Message = message; // A human-readable error message describing the issue.
            Detalis = detalis; // Optional technical details (e.g., stack trace or inner exception).
        }

        // The HTTP status code associated with the error.
        public int StatusCode { get; set; }

        // A human-readable error message for the client.
        public string Message { get; set; }

        // Optional technical details for debugging purposes.
        public string Detalis { get; set; }
    }
}

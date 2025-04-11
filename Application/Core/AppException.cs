namespace Application.Core
{
    // Represents a standardized structure for API error responses.
    // Typically used by global exception middleware to return consistent error details.

    public class AppException
    {
        // Constructor for setting the status code, message, and optional details
        public AppException(int statusCode, string message, string detalis = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detalis = detalis;
        }

        // HTTP status code (e.g. 400, 500)
        public int StatusCode { get; set; }

        // Human-readable error message
        public string Message { get; set; }

        // Optional technical details (e.g. stack trace or inner exception)
        public string Detalis { get; set; }
    }
}

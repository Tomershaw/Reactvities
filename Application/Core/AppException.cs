namespace Application.Core
{
    public class AppException
    {
        public AppException(int statusCode, string message, string detalis = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detalis = detalis;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        public string Detalis { get; set; }
    }
}
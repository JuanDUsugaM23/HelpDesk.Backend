namespace OPI.HelpDesk.Application.Dtos.Shares
{
    public class ResponseWrapper<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public ResponseWrapper(int statusCode, T result, bool success = true, string? message = null)
        {
            Result = result;
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
    }
}

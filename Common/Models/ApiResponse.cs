namespace Common.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public StatusCode StatusCode { get; set; }

        public ApiResponse(T data, string message = "", StatusCode statusCode = StatusCode.Success)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public enum StatusCode
    {
        Success,
        Error
    }
}

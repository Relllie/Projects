using System.Net;

namespace projects_api.Core.Models
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public ApiResult() { }
        public ApiResult(bool success, T data = default, string message = "", int statusCode = (int)HttpStatusCode.OK)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
    }
}

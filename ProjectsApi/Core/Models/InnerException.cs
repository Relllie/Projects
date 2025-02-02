using Microsoft.AspNetCore.Authentication.BearerToken;

namespace projects_api.Core.Models
{
    /// <summary>
    /// Внутренняя ошибка сервиса
    /// </summary>
    public class InnerException : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message {  get; set; }
        public InnerException(int errorCode, string message) 
        { 
            ErrorCode = errorCode;
            Message = message;
        }
    }
}

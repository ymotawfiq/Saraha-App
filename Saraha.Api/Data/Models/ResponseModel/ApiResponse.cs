namespace Saraha.Api.Data.Models.ResponseModel
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T? ResponseObject { get; set; }
    }
}

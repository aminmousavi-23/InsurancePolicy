namespace UserService.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Result { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public new T? Result { get; set; }
    }
}

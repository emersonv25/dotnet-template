namespace Template.Api.DTOs
{
    public abstract class ApiResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        protected ApiResponseDto(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class SuccessResponseDto<T> : ApiResponseDto
    {
        public T Data { get; set; }

        public SuccessResponseDto(int statusCode, string message, T data)
            : base(statusCode, message)
        {
            Data = data;
        }
    }

    public class ErrorResponseDto : ApiResponseDto
    {
        public IEnumerable<string>? Errors { get; set; }

        public ErrorResponseDto(int statusCode, string message, IEnumerable<string>? errors)
            : base(statusCode, message)
        {
            Errors = errors;
        }
    }
}

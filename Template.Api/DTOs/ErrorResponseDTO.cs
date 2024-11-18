namespace Template.Api.DTOs
{
    public class ErrorResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public ErrorResponseDTO(int statusCode, string message, IEnumerable<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}

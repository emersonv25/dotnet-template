using System.Text.Json;
using Template.Api.DTOs;

namespace Template.Api.Extension
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeaderDTO header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Append("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}

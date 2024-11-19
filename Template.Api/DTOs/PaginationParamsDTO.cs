using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Template.Api.DTOs
{
    public class PaginationParamsDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "The minimum page number is 1.")]
        [SwaggerSchema(Description = "The page number to retrieve.")]  // Exemplo no nível de propriedade
        public int PageNumber { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "The maximum page size is 50.")]
        [SwaggerSchema(Description = "The number of items per page.")]  // Exemplo no nível de propriedade
        public int PageSize { get; set; } = 10;
    }
}

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Template.Api.DTOs
{
    public class PaginationParamsDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "O número minimo de pagina é 1.")]
        [SwaggerSchema(Description = "O número da página a ser recuperada.")]  // Exemplo no nível de propriedade
        public int PageNumber { get; set; }

        [Range(1, 50, ErrorMessage = "O tamanho máximo da página é 50.")]
        [SwaggerSchema(Description = "O número de itens por página.")]  // Exemplo no nível de propriedade
        public int PageSize { get; set; }
    }
}

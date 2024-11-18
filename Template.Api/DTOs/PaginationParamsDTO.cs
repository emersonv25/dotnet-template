using System.ComponentModel.DataAnnotations;

namespace Template.Api.DTOs
{
    public class PaginationParamsDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "O número minimo por paginas é 1")]
        public int PageNumber { get; set; }
        [Range(1, 50, ErrorMessage = "O número máximo por pagina é 50")]
        public int PageSize { get; set; }
    }

    public class  PaginationHeaderDTO
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PaginationHeaderDTO(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
    }
}

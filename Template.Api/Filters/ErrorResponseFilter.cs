using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Template.Api.DTOs;

namespace Template.Api.Filters
{
    public class ErrorResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Esse método não precisa de implementação para o caso de manipulação do retorno.
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Verifica se a resposta é um erro (status 4xx ou 5xx)
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.StatusCode >= 400 && objectResult.StatusCode < 600)
                {
                    // Se a resposta for erro, cria um ErrorResponseDTO
                    var errorResponse = new ErrorResponseDTO(
                        objectResult.StatusCode ?? 500,  // Se o StatusCode for nulo, assume 500
                        GetErrorMessage(objectResult.StatusCode),
                        new[] { objectResult.Value?.ToString() ?? "An error occurred." }
                    );

                    // Substitui a resposta para o formato do ErrorResponseDTO
                    context.Result = new ObjectResult(errorResponse)
                    {
                        StatusCode = errorResponse.StatusCode
                    };
                }
            }
        }

        // Método auxiliar para mapear o código de status para uma mensagem padrão
        private string GetErrorMessage(int? statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                _ => "An error occurred"
            };
        }
    }
}

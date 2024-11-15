using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Template.Api.DTOs;

namespace Template.Api.Filters
{
    public class ApiResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nada a fazer antes da execução
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                // Verifique se o ObjectResult é do tipo OkObjectResult
                if (objectResult is OkObjectResult okObjectResult)
                {
                    // Aqui usamos um código de status padrão 200, mas você pode personalizar isso
                    var response = new SuccessResponseDto<object>(
                        StatusCodes.Status200OK,  // Código de status 200
                        "Request completed successfully",
                        okObjectResult.Value     // Dados retornados
                    );

                    // Atualiza o Result para encapsular a resposta no formato esperado
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = StatusCodes.Status200OK  // Garante que o código de status seja 200
                    };
                }
                else
                {
                    // Caso não seja OkObjectResult, você pode adicionar tratamento adicional aqui
                    // Ou apenas garantir que o status seja consistente
                    var response = new SuccessResponseDto<object>(
                        objectResult.StatusCode ?? StatusCodes.Status200OK, // Usar statusCode se disponível, ou 200
                        "Request completed successfully",
                        objectResult.Value
                    );

                    // Atualiza o Result para encapsular a resposta no formato esperado
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK
                    };
                }
            }

        }
    }
}

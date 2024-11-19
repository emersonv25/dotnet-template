using Microsoft.AspNetCore.Http;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using System.Threading.Tasks;
using Template.Api.DTOs;
using Template.Application.DTOs;
using FirebaseAdmin.Messaging;
using Template.Domain.Interfaces.Services;

namespace Template.Api.Middlewares
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFirebaseService _firebaseService;


        public FirebaseAuthMiddleware(RequestDelegate next, IFirebaseService firebaseService)
        {
            _next = next;
            _firebaseService = firebaseService;

        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            // Verifica se o token de autenticação está presente nos cabeçalhos da requisição.
            string token = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                // Se não houver token, apenas passa para o próximo middleware.
                await _next(context);
                return;
            }

            var tokenValue = ExtractToken(token);
            if (string.IsNullOrEmpty(tokenValue))
            {
                // Se o token não for válido, passa para o próximo middleware.
                await _next(context);
                return;
            }

            // Verifica o token Firebase e extrai o usuário.
            var firebaseUser = await AuthenticateTokenAsync(tokenValue);
            if (firebaseUser == null)
            {
                // Retorna resposta de erro 401 caso o token seja inválido ou expirado.
                await RespondUnauthorizedAsync(context, "Acesso Negado: Token inválido ou expirado.");
                return;
            }

            // Tenta buscar o usuário correspondente no sistema.
            var user = await userService.GetUserByFirebaseIdAsync(firebaseUser.Id);
            // Permitir acesso ao endpoint de criação/atualização de usuário mesmo que o usuário não esteja no banco.
            if (user == null)
            {
                var isUserCreateEndpoint = IsUserCreationOrUpdateEndpoint(context);


                if (!string.IsNullOrWhiteSpace(firebaseUser.Name) && !string.IsNullOrWhiteSpace(firebaseUser.Email))
                {
                    user = await userService.CreateOrUpdateUser(new UserDTO(firebaseUser.Name, firebaseUser.Email), firebaseUser.Id);

                    if(isUserCreateEndpoint)
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await context.Response.WriteAsJsonAsync(user);
                        return;
                    }
                }
                else if (!isUserCreateEndpoint)
                {
                    // Retorna 401 para outros endpoints.
                    await RespondUnauthorizedAsync(context, "Acesso Negado: Usuário não cadastrado");
                    return;
                }
            }

            // Adiciona o usuário ao contexto da requisição para ser acessado em outros lugares.
            context.Items["FirebaseId"] = firebaseUser.Id;
            context.Items["User"] = user;

            // Chama o próximo middleware na cadeia.
            await _next(context);
        }

        // Extrai o token do cabeçalho 'Authorization'.
        private static string ExtractToken(string token) => token.Replace("Bearer ", string.Empty).Trim();

        // Valida o token no Firebase e retorna o usuário.
        private async Task<FirebaseUserDTO?> AuthenticateTokenAsync(string tokenValue)
        {
            try
            {
                var decodedToken = await _firebaseService.VerifyIdTokenAsync(tokenValue);
                var email = decodedToken.Claims.TryGetValue("email", out var emailClaim) ? emailClaim.ToString() : string.Empty;
                var name = decodedToken.Claims.TryGetValue("name", out var nameClaim) ? nameClaim.ToString() : string.Empty;

                return new FirebaseUserDTO(decodedToken.Uid, email, name);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Firebase authentication failed: {ex.Message}");
                return null;
            }
        }

        // Retorna uma resposta de erro 401 Unauthorized com a mensagem fornecida.
        private static async Task RespondUnauthorizedAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var errorResponse = new ErrorResponseDTO(StatusCodes.Status401Unauthorized, message, null);

            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        // Verifica se a rota atual é o endpoint de criação/atualização de usuário.
        public static bool IsUserCreationOrUpdateEndpoint(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                // Tenta obter o ControllerActionDescriptor do metadata do endpoint.
                var controllerActionDescriptor = endpoint.Metadata
                    .OfType<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()
                    .FirstOrDefault();

                if (controllerActionDescriptor != null)
                {
                    // Verifica se o ActionName corresponde ao esperado.
                    return controllerActionDescriptor.ActionName.Equals("CreateOrUpdate", StringComparison.OrdinalIgnoreCase);
                }
            }

            // Como fallback, verifica o caminho diretamente.
            var path = context.Request.Path.Value ?? string.Empty;
            return path.Contains("CreateOrUpdate", StringComparison.OrdinalIgnoreCase);
        }
    }

}

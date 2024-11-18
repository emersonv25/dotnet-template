﻿using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Template.Api.DTOs;
using Template.Application.DTOs;

namespace Template.Api.Middlewares
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public FirebaseAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            // Verifica se o token de autenticação está presente nos cabeçalhos da requisição.
            if (!context.Request.Headers.TryGetValue("Authorization", out var token))
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
                await RespondUnauthorizedAsync(context, "Unauthorized: Invalid or expired token.");
                return;
            }

            // Tenta buscar o usuário correspondente no sistema.
            var user = await userService.GetUserByFirebaseIdAsync(firebaseUser.Id);
            if (user == null)
            {
                user = await userService.CreateOrUpdateUser(new UserDTO { Name = firebaseUser.Name, Email = firebaseUser.Email }, firebaseUser.Id);
                
                // Retorna erro 401 se o usuário não for encontrado.
                //await RespondUnauthorizedAsync(context, "Unauthorized: User not found.");
                //return;
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
                // Verifica o token Firebase de maneira assíncrona.
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenValue);
                var email = decodedToken.Claims.TryGetValue("email", out var emailClaim) ? emailClaim.ToString() : string.Empty;
                var name = decodedToken.Claims.TryGetValue("name", out var nameClaim) ? nameClaim.ToString() : string.Empty;
                // Retorna o usuário do Firebase.
                return new FirebaseUserDTO(decodedToken.Uid, email, name);
            }
            catch (FirebaseAuthException ex)
            {
                // Em caso de erro, logar ou capturar o erro (aqui você pode adicionar logging se necessário).
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
    }

}

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Template.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Template.Api.Utils;
using Template.Api;
using Microsoft.AspNetCore.Mvc;
using Template.Api.DTOs;
using Template.Api.Filters;


var builder = WebApplication.CreateBuilder(args);
var FirebaseCredentialPath = builder.Configuration["AppSettings:FirebaseCredentialPath"];

// Configuração Firebase
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(FirebaseCredentialPath)
});

// Registro dos serviços das camadas Application e Data
builder.Services.AddInfrastructure(builder.Configuration);

// Lê o arquivo JSON para obter o ProjectID
var FirebaseCredentialJson = File.ReadAllText(FirebaseCredentialPath);
var FirebaseCredentialJsonDoc = JsonDocument.Parse(FirebaseCredentialJson);
var Firebase_ProjectId = FirebaseCredentialJsonDoc.RootElement.GetProperty("project_id").GetString();

// Configuração do JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{Firebase_ProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{Firebase_ProjectId}",
            ValidateAudience = true,
            ValidAudience = $"{Firebase_ProjectId}",
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// Adiciona serviços de controle
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(e => e.Value.Errors.Select(x => x.ErrorMessage))
            .ToArray();

        var errorResponse = new ErrorResponseDto(StatusCodes.Status400BadRequest, "Validation failed", errors);

        return new BadRequestObjectResult(errorResponse);
    };
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TemplateApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using Bearer scheme",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<FirebaseAuthMiddleware>();

app.MapControllers();
app.Run();
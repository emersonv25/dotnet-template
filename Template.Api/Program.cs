using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Template.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Template.Api.DTOs;
using Template.Api.Filters;
using Template.Infra.Ioc;
using Template.Infra.Data.Services;


var builder = WebApplication.CreateBuilder(args);

// Registro dos serviços/repositorios
builder.Services.AddInfrastructure(builder.Configuration);

var firebaseService = new FirebaseService(builder.Configuration);

// Configuração do JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var firebaseProjectId = firebaseService.GetProjectId();

        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = $"{firebaseProjectId}",
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// Adiciona serviços de controle
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorResponseFilter>();

}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors?.Count > 0)
            .SelectMany(e => e.Value?.Errors?.Select(x => x.ErrorMessage) ?? Enumerable.Empty<string>())
            .ToArray();

        var errorResponse = new ErrorResponseDTO(StatusCodes.Status400BadRequest, "Validation failed", errors);

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
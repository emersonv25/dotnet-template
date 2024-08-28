using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Template.Api.Data;
using Template.Api.Filters;
using Template.Api.Repositories;
using Template.Api.Services;
using Template.Api.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FirebaseFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
string dbConnection = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));

    //options.UseLazyLoadingProxies();
});

//Firebase
string firebaseCrenditialPath = builder.Configuration["AppSettings:FirebaseCredentialPath"];
string firebaseAppId = builder.Configuration["AppSettings:FirebaseAppId"];

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebaseCrenditialPath)
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseAppId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseAppId}",
            ValidateAudience = true,
            ValidAudience = $"{firebaseAppId}",
            ValidateLifetime = true
        };
    });

//Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Template.Api",
        Version = "v1",
        Description = "",
        Contact = new OpenApiContact()
        {
            Name = "",
            Email = "",
            Url = new Uri("https://www.linkedin.com/in/emerson-de-jesus-santos-303640195/")
        },
    });
    // using System.Reflection;
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

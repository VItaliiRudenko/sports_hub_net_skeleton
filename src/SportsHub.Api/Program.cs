using System.Text.Json;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SportsHub.Api.Extensions;
using SportsHub.Api.Middlewares;
using SportsHub.Api.Services;
using SportsHub.Domain.Services;
using SportsHub.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

// Add services to the container.

var allowedHosts = "http://localhost:3000";//builder.Configuration["AllowedHosts"];
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(allowedHosts.Split(';', StringSplitOptions.RemoveEmptyEntries))
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("Content-Disposition")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // API Information
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SportsHub API",
        Version = "v1",
        Description = "A comprehensive API for managing sports articles, authentication, and file operations",
        Contact = new OpenApiContact
        {
            Name = "SportsHub Team",
            Email = "support@sportshub.example.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // JWT Security Configuration
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "BEARER",
        Name = "BEARER Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter **_ONLY_** your Bearer token in text-box below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

    // Configure tags for better organization
    setup.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    setup.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(bearerOptions =>
    {
        bearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey =
                new SymmetricSecurityKey("SazsdfasgfdgfsdfSazsdfasgfdgfsdfSazsdfasgfdgfsdfSazsdfasgfdgfsdf"u8.ToArray()),
            ValidIssuer = "https://auth.sportshub.example.com",
            ValidAudience = "https://app.sportshub.example.com",
        };
    });

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtDenyListMiddleware>();

builder.Services.AddScoped<IContextDataProvider, ContextDataProvider>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IArticlesService, ArticlesService>();
builder.Services.AddScoped<IApplicationMapper, ApplicationMapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    await app.SeedInitialData();
}

app.UseAuthorization();

app.UseMiddleware<JwtDenyListMiddleware>();

app.UseCors();

app.MapControllers();

app.Run();

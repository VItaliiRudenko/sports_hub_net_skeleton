using System.Text.Json;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SportsHub.Api.Extensions;
using SportsHub.Api.Middlewares;
using SportsHub.Api.Services;
using SportsHub.Api.Models.Configuration;
using SportsHub.Domain.Services;
using SportsHub.Infrastructure.Db;
using System.Text;
using SportsHub.Api.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

// Bind configuration settings
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings are not configured properly");

var corsSettings = builder.Configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>()
    ?? throw new InvalidOperationException("CORS settings are not configured properly");

// Validate critical settings
if (string.IsNullOrEmpty(jwtSettings.SecretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey is not configured. Please set it via User Secrets in development or Environment Variables in production.");
}

if (jwtSettings.SecretKey.Length < 32)
{
    throw new InvalidOperationException("JWT SecretKey must be at least 32 characters long for security.");
}

// Add services to the container.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(corsSettings.GetAllowedOriginsArray())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("Content-Disposition")));

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ModelValidationFilter>();
    })
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtDenyListMiddleware>();

// Register configuration settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection(CorsSettings.SectionName));

builder.Services.AddScoped<IContextDataProvider, ContextDataProvider>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IArticlesService, ArticlesService>();
builder.Services.AddScoped<ILanguagesService, LanguagesService>();
builder.Services.AddScoped<IApplicationMapper, ApplicationMapper>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("SportsHubDb") ?? string.Empty, name: "database")
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

var app = builder.Build();

// Add global exception handling as the first middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

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

// Add health check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.Run();

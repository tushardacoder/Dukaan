using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Dukaan.Infrastructure.Services;
using Dukaan.Infrastructure.Data.Model;
using Dukaan.Infrastructure.Data.DbContext;
using Dukaan.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Service Registration Section ---
// This is where we register dependencies for the built-in Dependency Injection (DI) container.

// Register the Database Context with PostgreSQL support
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register ASP.NET Core Identity for authentication
builder.Services.AddIdentity<Merchant, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register application-specific services and repositories
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped(typeof(Repository<>)); // Registers the generic repository

//register Iauthservice with authservice
builder.Services.AddScoped<IAuthService,AuthService>();

// Register OpenAPI (Swagger) for API documentation
builder.Services.AddOpenApi();

// Register MVC controllers
builder.Services.AddControllers();


// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// --- 2. Middleware Pipeline Section ---
// This defines the order in which HTTP requests are processed.

if (app.Environment.IsDevelopment())
{
    // Enables the interactive Swagger UI in development mode
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();

// Maps controller routes (e.g., [Route("api/[controller]")])
app.MapControllers();

// Starts the application
app.Run();
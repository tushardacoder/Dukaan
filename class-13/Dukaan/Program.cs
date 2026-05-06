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

//
// ─────────────────────────────────────
// 1. DATABASE (PostgreSQL)
// ─────────────────────────────────────
//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

//
// ─────────────────────────────────────
// 2. IDENTITY SETUP
// ─────────────────────────────────────
//
builder.Services.AddIdentity<Merchant, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//
// ─────────────────────────────────────
// 3. APPLICATION SERVICES
// ─────────────────────────────────────
//
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<MerchantRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();


//
// ─────────────────────────────────────
// 4. CONTROLLERS + OPENAPI
// ─────────────────────────────────────
//
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//
// ─────────────────────────────────────
// 5. JWT AUTHENTICATION
// ─────────────────────────────────────
//
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

//
// ─────────────────────────────────────
// 6. AUTHORIZATION
// ─────────────────────────────────────
//
builder.Services.AddAuthorization();

//
// ─────────────────────────────────────
// BUILD APP
// ─────────────────────────────────────
//
var app = builder.Build();

//
// ─────────────────────────────────────
// 7. MIDDLEWARE PIPELINE 
// ─────────────────────────────────────
//

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
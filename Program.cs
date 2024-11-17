using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MMS.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "DefaultSecureKey"; // Load key from appsettings.json or use a default for development
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException(nameof(jwtKey), "JWT key is not configured in appsettings.json or environment variables.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    {
        Console.WriteLine("Invalid or missing Authorization Header.");
        if (!string.IsNullOrEmpty(authHeader))
        {
            Console.WriteLine($"Received Authorization Header: {authHeader}");
        }
    }
    else
    {
        Console.WriteLine("Valid Authorization Header detected.");
        Console.WriteLine($"Authorization Header: {authHeader}");
    }

    await next();
});


app.UseAuthorization();

app.MapControllers();

app.Run();

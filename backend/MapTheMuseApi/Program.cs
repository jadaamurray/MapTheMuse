using MapTheMuseApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MapTheMuseApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MapTheMuseApi.Controllers;
using Npgsql;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// ability to deserialise dictionaries
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
// EF Core and Identity
builder.Services.AddDbContext<MapTheMuseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<MapTheMuseContext>().AddDefaultTokenProviders();
// Email services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();
// JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

// Interfaces
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IDestinationMediaService, DestinationMediaService>();
// Adding CORS services
var allowedFromConfig = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

var csv = builder.Configuration["Cors:AllowedOriginsCsv"];
if (!string.IsNullOrWhiteSpace(csv))
{
    allowedFromConfig = allowedFromConfig
        .Concat(csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();
}    
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrEmpty(origin)) return false;

                // Exact allow-list from config (easiest for prod)
                if (allowedFromConfig.Contains(origin, StringComparer.OrdinalIgnoreCase))
                    return true;

                // Allow any Vercel preview like https://pr-123-map-the-muse.vercel.app
                var host = new Uri(origin).Host;
                if (host.EndsWith(".vercel.app", true, CultureInfo.InvariantCulture))
                    return true;

                return false;
            })
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// tmdb api
builder.Services.AddHttpClient<TmdbClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["TMDB:BaseUrl"]!);
    var bearer = builder.Configuration["TMDB:Bearer"];
    if (!string.IsNullOrWhiteSpace(bearer))
    {
        c.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearer);
    }
});


builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// Auto-apply EF migrations on boot
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MapTheMuseContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));


app.Run();
public partial class Program { }
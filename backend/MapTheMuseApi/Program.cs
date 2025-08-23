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
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

// ability to deserialise dictionaries
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
// EF Core
builder.Services.AddDbContext<MapTheMuseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 6;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._";
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddPasswordValidator<NotSameAsCurrentPasswordValidator<AppUser>>()
    .AddEntityFrameworkStores<MapTheMuseContext>()
    .AddDefaultTokenProviders();
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

// ---- Interfaces ----
// services
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IDestinationMediaService, DestinationMediaService>();
builder.Services.AddScoped<IMediaSpineService, MediaSpineService>();
builder.Services.AddScoped<IFavouritesService, FavouritesService>();
builder.Services.AddScoped<IMediaService, MediaService>();

// repositories
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();

// Adding CORS services
var allowed = (builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
    .Concat((builder.Configuration["Cors:AllowedOriginsCsv"] ?? string.Empty)
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

        // Fail fast (or provide a dev fallback)
if (allowed.Length == 0)
{
    throw new InvalidOperationException(
        "CORS:AllowedOrigins is empty.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowed)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
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

// --- Sharing services ---
// http image cache
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IImageFetcher, HttpImageFetcher>();
// collage renderer
builder.Services.AddSingleton<CollageRenderer>();
// collage query
builder.Services.AddScoped<CollageQuery>();
// health services
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
    // Tag DB check as "ready" so we can filter for /readyz
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" });

// Rate limiting
builder.Services.AddRateLimiter(opts =>
{
    opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress ?? IPAddress.IPv6Loopback;
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,          // 100 req
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });
    // Exempt health and static files
    opts.AddPolicy("NoLimit", context => RateLimitPartition.GetNoLimiter(string.Empty));
});

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
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
// JSON response writer
static Task WriteHealthJson(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(kvp => new
        {
            name = kvp.Key,
            status = kvp.Value.Status.ToString(),
            duration = kvp.Value.Duration.TotalMilliseconds,
            error = kvp.Value.Exception?.Message,
            description = kvp.Value.Description,
            tags = kvp.Value.Tags
        })
    };

    var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
    {
        WriteIndented = false
    });

    return context.Response.WriteAsync(json);
}

// exception handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error"); // map a minimal /error endpoint that returns ProblemDetails
}

// forwarded headers in production
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        // TODO: add KnownProxies/Networks if you can
    });
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseRateLimiter();

// security headers
app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "DENY";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    h["Cross-Origin-Opener-Policy"] = "same-origin";
    h["Cross-Origin-Resource-Policy"] = "same-site";
    await next();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// /livez – only self check
app.MapHealthChecks("/livez", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live"),
    ResponseWriter = WriteHealthJson
})
    .RequireRateLimiting("NoLimit")
    .AllowAnonymous();

// /readyz – checks required to serve traffic (DB, external deps)
// Tag those checks with "ready"
app.MapHealthChecks("/readyz", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready"),
    ResponseWriter = WriteHealthJson
})
    .RequireRateLimiting("NoLimit")
    .AllowAnonymous();

// /healthz – everything
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = WriteHealthJson
})
    .RequireRateLimiting("NoLimit")
    .AllowAnonymous();

app.Run();
public partial class Program { }
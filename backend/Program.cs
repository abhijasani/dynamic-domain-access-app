
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
       {
           options.AddPolicy("DynamicCors", policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true) // let middleware control it
                    .AllowCredentials();
            });
       });

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Host=postgres;Database=DynamicCorsDb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<OriginService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Automatically apply any pending migrations
    await context.Database.MigrateAsync();

    // Optional: Seed default origins
    if (!await context.AllowedOrigins.AnyAsync())
    {
        // context.AllowedOrigins.AddRange(new[]
        // {
        //     new AllowedOrigin { Origin = "http://localhost:4200", Status = "Approved" },
        //     new AllowedOrigin { Origin = "http://localhost:3000", Status = "Approved" }
        // });
        await context.SaveChangesAsync();
    }
}


app.MapOpenApi();
app.UseRouting();
app.UseCors("DynamicCors");
app.UseMiddleware<DynamicCorsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


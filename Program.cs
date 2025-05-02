using Microsoft.EntityFrameworkCore;
using LogAnalysisApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Rejestrujemy DbContext z połączeniem z appsettings.json
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

// Prosty testowy endpoint
app.MapGet("/test", () => Results.Ok(new { message = "API działa!" }));

// Endpoint do pobierania logów z bazy danych
app.MapGet("/logs", async (LogDbContext db) =>
    await db.Logs.ToListAsync());

// Endpoint do dodawania logów
app.MapPost("/logs", async (LogEntry log, LogDbContext db) =>
{
    db.Logs.Add(log);
    await db.SaveChangesAsync();
    return Results.Created($"/logs/{log.Id}", log);
});

app.Run();


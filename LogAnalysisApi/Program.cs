using LogAnalysisApi.Data;
using LogAnalysisApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Swagger + API explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj Entity Framework + MySQL
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// 🌐 Domyślne endpointy testowe
app.MapGet("/", () => Results.Ok("Tu działa API logów!"));
app.MapGet("/test", () => Results.Ok(new { message = "API działa!" }));

// 🧾 Endpointy logów
app.MapGet("/logs", async (LogDbContext db) =>
    await db.Logs.ToListAsync());

app.MapPost("/logs", async (LogEntry log, LogDbContext db) =>
{
    log.Timestamp = DateTime.UtcNow;
    db.Logs.Add(log);
    await db.SaveChangesAsync();
    return Results.Created($"/logs/{log.Id}", log);
});

app.Run();

// Rekord forecastu
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


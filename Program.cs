//Med hjälp av AI skriven enkel kod för att skapa en REST API med ASP.NET Core och Entity Framework Core som använder SQLite som databas. Den innehåller en databasmodell för kurser, en databas-kontroller och två API-endpoints för att hämta och lägga till kurser.

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Databas-konfiguration (SQLite)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=utbildning.db"));
builder.Services.AddCors(); // Tillåt frontend att anropa backend

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// 2. Databasmodeller
public record Course(int Id, string Name, string Teacher);
class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Course> Courses => Set<Course>();
}

// 3. Skapa databasen vid start
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 4. API Endpoints
app.MapGet("/courses", async (AppDbContext db) => await db.Courses.ToListAsync());

app.MapPost("/courses", async (AppDbContext db, Course course) => {
    db.Courses.Add(course);
    await db.SaveChangesAsync();
    return Results.Created($"/courses/{course.Id}", course);
});

app.Run();
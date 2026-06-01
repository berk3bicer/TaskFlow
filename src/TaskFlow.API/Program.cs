using Microsoft.EntityFrameworkCore;
using TaskFlow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────
// SERVICES
// ──────────────────────────────────────────────────────

builder.Services.AddDbContext<TaskFlowDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// ──────────────────────────────────────────────────────
// MIDDLEWARE PIPELINE
// ──────────────────────────────────────────────────────

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
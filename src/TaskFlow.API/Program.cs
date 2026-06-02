using TaskFlow.Application;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────
// SERVICES
// ──────────────────────────────────────────────────────

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// ──────────────────────────────────────────────────────
// MIDDLEWARE PIPELINE
// ──────────────────────────────────────────────────────

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
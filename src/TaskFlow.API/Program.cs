using Scalar.AspNetCore;
using TaskFlow.API.Common;
using TaskFlow.Application;
using TaskFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────────────
// SERVICES
// ──────────────────────────────────────────────────────

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ──────────────────────────────────────────────────────
// MIDDLEWARE PIPELINE
// ──────────────────────────────────────────────────────

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
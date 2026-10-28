using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Data;
using ReviewInsights.Api.Repositories;
using ReviewInsights.Api.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository Layer (Data Access)
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Register Service Layer (Business Logic)
builder.Services.AddScoped<IReviewService, ReviewService>();

// Register External Services
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IReviewAnalysisService, GeminiReviewAnalysisService>();

var app = builder.Build();

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

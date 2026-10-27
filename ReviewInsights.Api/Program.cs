using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Data;
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


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ReviewService>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<GeminiReviewAnalysisService>();

var app = builder.Build();

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Data;
using ReviewInsights.Api.Services;

var builder = WebApplication.CreateBuilder( );

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ReviewService>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<GeminiReviewAnalysisService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

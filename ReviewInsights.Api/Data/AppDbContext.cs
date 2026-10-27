using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Models;

namespace ReviewInsights.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Review> Reviews => Set<Review>();
}
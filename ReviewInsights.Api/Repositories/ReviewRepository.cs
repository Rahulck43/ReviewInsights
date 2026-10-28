using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Data;
using ReviewInsights.Api.Models;

namespace ReviewInsights.Api.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(AppDbContext context, ILogger<ReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all reviews from database");

        try
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            _logger.LogInformation("Successfully fetched {Count} reviews", reviews.Count);
            return reviews;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching reviews from database");
            throw;
        }
    }

    public async Task<Review> AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding new review to database with ID: {ReviewId}", review.Id);

        try
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully saved review with ID: {ReviewId}", review.Id);
            return review;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving review with ID: {ReviewId}", review.Id);
            throw;
        }
    }
}

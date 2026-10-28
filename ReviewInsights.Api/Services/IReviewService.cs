using ReviewInsights.Api.Models;

namespace ReviewInsights.Api.Services;

public interface IReviewService
{
    Task<List<Review>> GetAllAsync();
    Task<Review> CreateAsync(string reviewText, CancellationToken cancellationToken = default);
}

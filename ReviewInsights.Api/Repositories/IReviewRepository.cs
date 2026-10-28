using ReviewInsights.Api.Models;

namespace ReviewInsights.Api.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync();
    Task<Review> AddAsync(Review review, CancellationToken cancellationToken = default);
}

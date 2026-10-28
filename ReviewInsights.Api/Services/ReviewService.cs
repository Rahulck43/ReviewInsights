using ReviewInsights.Api.Models;
using ReviewInsights.Api.Repositories;

namespace ReviewInsights.Api.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IReviewAnalysisService _analysisService;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(
        IReviewRepository reviewRepository,
        IReviewAnalysisService analysisService,
        ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _analysisService = analysisService;
        _logger = logger;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all reviews");
        
        try
        {
            var reviews = await _reviewRepository.GetAllAsync();
            _logger.LogInformation("Successfully retrieved {Count} reviews", reviews.Count);
            return reviews;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve reviews");
            throw;
        }
    }

    public async Task<Review> CreateAsync(
        string reviewText,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new review with text length: {Length}", reviewText.Length);
        
        try
        {
            // 1. Analyze the review using AI service
            _logger.LogDebug("Starting AI analysis for review");
            var analysis = await _analysisService.AnalyzeAsync(
                reviewText, cancellationToken);

            // 2. Create review entity with AI analysis results
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ReviewText = reviewText,
                Sentiment = analysis.Sentiment,
                Category = analysis.Category,
            Summary = analysis.Summary,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Persist to database through repository
            _logger.LogDebug("Saving review to database");
            var savedReview = await _reviewRepository.AddAsync(review, cancellationToken);

            _logger.LogInformation("Successfully created review with ID: {ReviewId}", savedReview.Id);
            return savedReview;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create review");
            throw;
        }
    }
}
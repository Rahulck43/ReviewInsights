using Microsoft.EntityFrameworkCore;
using ReviewInsights.Api.Data;
using ReviewInsights.Api.DTOs;
using ReviewInsights.Api.Models;

namespace ReviewInsights.Api.Services;

public class ReviewService
{
    private readonly AppDbContext _context;
    private readonly GeminiReviewAnalysisService _geminiService;

    public ReviewService(AppDbContext context,
        GeminiReviewAnalysisService geminiService)
    {
        _context = context;
        _geminiService = geminiService;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review> CreateAsync(
        string reviewText,
        CancellationToken cancellationToken = default)
    {
        // 1. Analyze the review using Gemini.
        var analysis = await _geminiService.AnalyzeAsync(
            reviewText, cancellationToken);

        // 2. Save the original review and AI results together.
        var review = new Review
        {
            ReviewText = reviewText,
            Sentiment = analysis.Sentiment,
            Category = analysis.Category,
            Summary = analysis.Summary,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        return review;
    }
}
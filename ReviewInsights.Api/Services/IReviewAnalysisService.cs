using ReviewInsights.Api.DTOs;

namespace ReviewInsights.Api.Services;

public interface IReviewAnalysisService
{
    Task<ReviewAnalysisResult> AnalyzeAsync(string reviewText, CancellationToken cancellationToken = default);
}

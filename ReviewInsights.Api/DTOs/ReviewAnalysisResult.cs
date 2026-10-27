namespace ReviewInsights.Api.DTOs;

public class ReviewAnalysisResult
{
    public string Sentiment { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;
}

namespace ReviewInsights.Api.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ReviewText { get; set; } = string.Empty;

    public string Sentiment { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
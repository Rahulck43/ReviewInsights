using System.ComponentModel.DataAnnotations;

namespace ReviewInsights.Api.DTOs;

public class CreateReviewRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 3)]
    public string ReviewText { get; set; } = string.Empty;
}

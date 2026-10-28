using Microsoft.AspNetCore.Mvc;
using ReviewInsights.Api.DTOs;
using ReviewInsights.Api.Services;

namespace ReviewInsights.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Received request to get all reviews");
        
        try
        {
            var reviews = await _reviewService.GetAllAsync();
            _logger.LogInformation("Successfully returned {Count} reviews", reviews.Count);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching reviews");
            return StatusCode(500, new { error = "An error occurred while fetching reviews" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request)
    {
        _logger.LogInformation("Received request to create new review");
        
        if (request == null || string.IsNullOrWhiteSpace(request.ReviewText))
        {
            _logger.LogWarning("Invalid create review request: empty or null review text");
      return BadRequest(new { error = "Review text is required" });
        }
        
        try
        {
            var review = await _reviewService.CreateAsync(
                request.ReviewText.Trim(),
                HttpContext.RequestAborted);

            _logger.LogInformation("Successfully created review with ID: {ReviewId}", review.Id);
            
         return CreatedAtAction(
                nameof(GetAll),
                new { id = review.Id },
                review);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating review");
            return StatusCode(500, new { error = "An error occurred while creating the review" });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using ReviewInsights.Api.DTOs;
using ReviewInsights.Api.Services;

namespace ReviewInsights.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _reviewService.GetAllAsync();
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request)
    {
        var review = await _reviewService.CreateAsync(
            request.ReviewText.Trim(),
            HttpContext.RequestAborted);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = review.Id },
            review);
    }
}
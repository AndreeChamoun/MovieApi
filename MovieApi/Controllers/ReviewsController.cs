using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTO;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext _context;

        public ReviewsController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/movies/{movieId}/reviews
        [HttpGet("movies/{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews(int movieId)
        {
            var movie = await _context.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                return NotFound();

            var reviews = movie.Reviews
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
                .ToList();

            return Ok(reviews);
        }

        // POST: api/movies/{movieId}/reviews
        [HttpPost("movies/{movieId}/reviews")]
        public async Task<ActionResult<ReviewDto>> PostReview(int movieId, ReviewDto reviewDto)
        {
            var movie = await _context.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                return NotFound();

            var review = new Models.Review
            {
                ReviewerName = reviewDto.ReviewerName,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating
            };

            movie.Reviews.Add(review);
            await _context.SaveChangesAsync();

            reviewDto.Id = review.Id;

            return CreatedAtAction(nameof(GetReviews), new { movieId = movieId }, reviewDto);
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
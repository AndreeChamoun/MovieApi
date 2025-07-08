using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using MovieApi.Models.DTO;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieDetails)
                .ToListAsync();

            var moviesDto = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Genre = m.Genre,
                Duration = m.Duration
            }).ToList();

            return moviesDto;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration
            };

            return dto;
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Include(m => m.Reviews)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration,
                MovieDetails = movie.MovieDetails == null ? null : new MovieDetailsDto
                {
                    Id = movie.MovieDetails.Id,
                    Synopsis = movie.MovieDetails.Synopsis,
                    Language = movie.MovieDetails.Language,
                    Budget = movie.MovieDetails.Budget
                },
                Reviews = movie.Reviews?.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                }).ToList() ?? new List<ReviewDto>(),
                Actors = movie.MovieActors?.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    Name = ma.Actor.Name,
                    BirthYear = ma.Actor.BirthYear
                }).ToList() ?? new List<ActorDto>()
            };

            return dto;
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Genre = dto.Genre,
                Duration = dto.Duration,
                MovieDetails = dto.MovieDetails == null ? null : new MovieDetails
                {
                    Synopsis = dto.MovieDetails.Synopsis,
                    Language = dto.MovieDetails.Language,
                    Budget = dto.MovieDetails.Budget
                }
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Returnera MovieDto
            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration
            };

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDto);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Uppdatera fält
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Genre = dto.Genre;
            movie.Duration = dto.Duration;

            // Uppdatera details om det finns
            if (dto.MovieDetails != null)
            {
                if (movie.MovieDetails == null)
                {
                    movie.MovieDetails = new MovieDetails();
                }
                movie.MovieDetails.Synopsis = dto.MovieDetails.Synopsis;
                movie.MovieDetails.Language = dto.MovieDetails.Language;
                movie.MovieDetails.Budget = dto.MovieDetails.Budget;
            }
            else if (movie.MovieDetails != null)
            {
                _context.MovieDetails.Remove(movie.MovieDetails);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            if (movie.MovieDetails != null)
            {
                _context.MovieDetails.Remove(movie.MovieDetails);
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
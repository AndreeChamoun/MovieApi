using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.DTO;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly MovieContext _context;

        public ActorsController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            var actors = await _context.Actors
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthYear = a.BirthYear
                })
                .ToListAsync();

            return Ok(actors);
        }

        // GET: api/actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _context.Actors
                .Where(a => a.Id == id)
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthYear = a.BirthYear
                })
                .FirstOrDefaultAsync();

            if (actor == null)
                return NotFound();

            return Ok(actor);
        }

        // POST: api/actors
        [HttpPost]
        public async Task<ActionResult<ActorDto>> PostActor(ActorDto actorDto)
        {
            var actor = new Models.Actor
            {
                Name = actorDto.Name,
                BirthYear = actorDto.BirthYear
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            actorDto.Id = actor.Id;

            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actorDto);
        }

        // PUT: api/actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, ActorDto actorDto)
        {
            if (id != actorDto.Id)
                return BadRequest();

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
                return NotFound();

            actor.Name = actorDto.Name;
            actor.BirthYear = actorDto.BirthYear;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/actors/id
        [HttpPost("{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null)
                return NotFound($"Movie with id {movieId} not found.");

            var actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
                return NotFound($"Actor with id {actorId} not found.");

            // Kontrollera så att aktören inte redan är kopplad till filmen
            if (movie.MovieActors.Any(ma => ma.ActorId == actorId))
                return BadRequest("Actor is already added to this movie.");

            movie.MovieActors.Add(new MovieApi.Models.MovieActor
            {
                MovieId = movieId,
                ActorId = actorId
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
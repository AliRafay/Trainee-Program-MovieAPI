using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.WebAPI.Entities;
using Movies.WebAPI.Requests;
using Movies.WebAPI.Responses;

namespace Movies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        MovieDbContext dbContext;
        public MovieController(MovieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public List<MovieResponse> GetMovies(string? searchKeyword, int pageNumber = 1, int count=10)
        {
            var query = dbContext.Movies.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchKeyword))
                query = query.Where(movie => movie.Title.Contains(searchKeyword));

            return query
                .OrderBy(movie => movie.Title)
                .Select(movie => new MovieResponse
                {
                    Id = movie.Id,
                    Year = movie.Year,
                    Title = movie.Title,
                    Rating = movie.Rating,
                    Genre = movie.Genre.ToString(),
                })
                .Skip((pageNumber-1) * count)
                .Take(count)
                .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<MovieResponse> GetMovie(int id)
        {
            var movie = dbContext.Movies
                .Select(movie => new MovieResponse
                {
                    Id = movie.Id,
                    Year = movie.Year,
                    Title = movie.Title,
                    Rating = movie.Rating,
                    Genre = movie.Genre.ToString(),
                })
                .FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound("Movie Not Found.");
            return movie;

        }

        [HttpPost]
        public ActionResult<MovieResponse> CreateMovie(MovieRequest request)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Director = request.Director,
                Year = request.Year,
                Genre = request.Genre,
                Rating = request.Rating,
            };
            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            var result = new MovieResponse
            {
                Id = movie.Id,
                Year = movie.Year,
                Title = movie.Title,
                Director = movie.Director,
                Rating = movie.Rating,
                Genre = movie.Genre.ToString(),
            };
            return Created(result.Id.ToString(), result);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> UpdateMovie(int id, MovieRequest request)
        {
            var movie = dbContext.Movies
                .FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound("Movie Not Found.");
            movie.Year = request.Year;
            movie.Genre = request.Genre;
            movie.Rating = request.Rating;
            movie.Title = request.Title;

            dbContext.Update(movie);
            dbContext.SaveChanges();
            return true;

        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteMovie(int id)
        {
            var movie = dbContext.Movies
                .FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound("Movie Not Found.");
            dbContext.Remove(movie);
            dbContext.SaveChanges();
            return true;

        }
    }
}

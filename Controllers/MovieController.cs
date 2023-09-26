using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.WebAPI.Entities;
using Movies.WebAPI.ExternalDTOs;
using Movies.WebAPI.Requests;
using Movies.WebAPI.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Movies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        MovieDbContext dbContext;
        IConfiguration configuration;
        public MovieController(MovieDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public List<MovieResponse> GetMovies(string? searchKeyword, int pageNumber = 1, int count=10)
        {
            var query = dbContext.Movies.AsQueryable();
            
            if (!string.IsNullOrEmpty(searchKeyword))
                query = query.Where(movie => movie.Title.Contains(searchKeyword));

            var result = query
                .OrderBy(movie => movie.Title)
                .Select(movie => new
                {
                    movie.Actors,
                    movieResponse = new MovieResponse
                    {
                        Id = movie.Id,
                        Year = movie.Year,
                        Title = movie.Title,
                        Rating = movie.Rating,
                        Genre = movie.Genre.ToString(),
                    }
                })
                .Skip((pageNumber - 1) * count)
                .Take(count)
                .ToList();

            result.ForEach(movie => movie.movieResponse.ActorNames = movie.Actors.Select(actor => actor.Name).ToList());

            return result.Select(r => r.movieResponse).ToList();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
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
        [Authorize(Roles = UserRoles.Admin)]
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
        [Authorize(Roles = UserRoles.Admin)]
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
        [Authorize(Roles = UserRoles.Admin)]
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

        [HttpGet("top-rated")]
        [AllowAnonymous]
        public ActionResult<List<MovieResponse>> GetTopRatedMovies(int pageNumber)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", configuration["MovieDBApiKey"]);
            var response = httpClient.GetAsync($"https://api.themoviedb.org/3/movie/top_rated?language=en-US&page={pageNumber}").Result;
            var stringResult = response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : response.StatusCode.ToString();
            var topRatedMovieResponse = JsonConvert.DeserializeObject<TopRatedMovieResponse>(stringResult);
            return topRatedMovieResponse.Results.Select(r => new MovieResponse
            {
                Title = r.Title,
                Year = int.Parse(r.ReleaseDate.Split('-')[0]),
                Rating = r.VoteAverage,
            }).ToList();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.WebAPI.Entities;

namespace Movies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private DbContextOptionsBuilder<MovieDbContext> optionsBuilder;
        public MovieController()
        {
            optionsBuilder = new DbContextOptionsBuilder<MovieDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=MovieDb;Username=postgres;Password=Click123$");
        }
        [HttpGet]
        public List<Movie> GetMovies()
        {
            // Create an instance of MyDbContext
            using (var dbContext = new MovieDbContext(optionsBuilder.Options))
            {
                dbContext.Database.EnsureCreated();
                return dbContext.Movies.ToList();
            }
        }
    }
}

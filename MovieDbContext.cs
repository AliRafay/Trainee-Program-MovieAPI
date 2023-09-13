using Microsoft.EntityFrameworkCore;
using Movies.WebAPI.Entities;

namespace Movies.WebAPI
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {}

        public DbSet<Movie> Movies { get; set; }
    }
}

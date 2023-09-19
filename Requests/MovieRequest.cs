using System.ComponentModel.DataAnnotations;

namespace Movies.WebAPI.Requests
{
    public class MovieRequest
    {
        [Required] public string Title { get; set; }
        [Required] public string Director { get; set; }
        [Required] public MovieGenre Genre { get; set; }
        [Range(1900, 2023)] public int Year { get; set; }
        [Range(0, 10)] public double Rating { get; set; }
    }

    public enum MovieGenre
    {
        Comedy = 1,
        Action,
        Drama,
        Thriller,
        Horror
    }
}

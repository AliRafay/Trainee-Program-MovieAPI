using Movies.WebAPI.Requests;

namespace Movies.WebAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public MovieGenre Genre { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public List<Actor> Actors { get; set; }
    }
}

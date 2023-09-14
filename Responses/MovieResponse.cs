using Movies.WebAPI.Requests;

namespace Movies.WebAPI.Responses
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
    }
}

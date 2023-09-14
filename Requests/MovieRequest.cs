namespace Movies.WebAPI.Requests
{
    public class MovieRequest
    {
        public string Title { get; set; }
        public MovieGenre Genre { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
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

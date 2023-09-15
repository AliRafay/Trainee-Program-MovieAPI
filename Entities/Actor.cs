namespace Movies.WebAPI.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}

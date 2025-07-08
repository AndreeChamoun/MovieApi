namespace MovieApi.Models.DTO
{
    public class MovieUpdateDto
    {
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }
        public MovieDetailsCreateDto? MovieDetails { get; set; }
    }
}

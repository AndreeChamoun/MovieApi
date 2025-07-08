using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Range(1900, 2100)]
        public int Year { get; set; }
        [Required]
        public string Genre { get; set; } = null!;
        [Range(1, 500)]
        public int Duration { get; set; }

        public MovieDetailsCreateDto? MovieDetails { get; set; }
    }

    public class MovieDetailsCreateDto
    {
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public decimal Budget { get; set; }
    }
}
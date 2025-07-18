﻿using System.Collections.Generic;

namespace MovieApi.Models.DTO
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }
        public MovieDetailsDto? MovieDetails { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
        public List<ActorDto> Actors { get; set; } = new();
    }
}

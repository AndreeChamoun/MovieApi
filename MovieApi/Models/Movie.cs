﻿using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }

        public MovieDetails? MovieDetails { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<MovieActor>? MovieActors { get; set; }
    }
}
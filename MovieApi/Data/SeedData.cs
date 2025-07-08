using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

            if (context.Movies.Any())
                return; 

            // Skådespelare
            var actor1 = new Actor { Name = "Robert De Niro", BirthYear = 1943 };
            var actor2 = new Actor { Name = "Al Pacino", BirthYear = 1940 };
            var actor3 = new Actor { Name = "Natalie Portman", BirthYear = 1981 };

            context.Actors.AddRange(actor1, actor2, actor3);

            // Filmer
            var movie1 = new Movie
            {
                Title = "Heat",
                Year = 1995,
                Genre = "Crime",
                Duration = 170,
                MovieDetails = new MovieDetails
                {
                    Synopsis = "A group of professional bank robbers start to feel the heat from police.",
                    Language = "English",
                    Budget = 60000000m
                },
                Reviews = new List<Review>
                {
                    new Review { ReviewerName = "Kalle", Comment = "Fantastisk film!", Rating = 5 },
                    new Review { ReviewerName = "Lisa", Comment = "Välspelad men lite lång.", Rating = 4 }
                },
                MovieActors = new List<MovieActor>
                {
                    new MovieActor{ Actor = actor1 },
                    new MovieActor{ Actor = actor2 },
                    new MovieActor{ Actor = actor3 }
                }
            };

            var movie2 = new Movie
            {
                Title = "The Godfather",
                Year = 1972,
                Genre = "Crime",
                Duration = 175,
                MovieDetails = new MovieDetails
                {
                    Synopsis = "The aging patriarch of an organized crime dynasty transfers control to his son.",
                    Language = "English",
                    Budget = 6000000m
                },
                Reviews = new List<Review>
                {
                    new Review { ReviewerName = "Anna", Comment = "En klassiker!", Rating = 5 }
                },
                MovieActors = new List<MovieActor>
                {
                    new MovieActor{ Actor = actor2 }
                }
            };

            context.Movies.AddRange(movie1, movie2);

            context.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Zengenti.Contensis.Management;

namespace MovieImport
{
    class Program
    {
        // The mapping between the taxonomy node name in the CSV and the taxonomy key to save
        private static readonly Dictionary<string, string> GenreTaxonomyKeys = new Dictionary<string, string>
        {
            {"Crime", "0/24/25/26"},
            {"Drama", "0/24/25/27"},
            {"Action", "0/24/25/28"},
            {"Western", "0/24/25/29"},
            {"Comedy", "0/24/25/30"},
            {"Romance", "0/24/25/31"}
        };

        static void Main()
        {
            // Create the management client
            var client = ManagementClient.Create("<Contensis URL>",
                "<Client ID>",
                "<Shared Secret>");

            // Get the project
            var project = client.Projects.Get("<Project Id>");

            // Read the movie CSV file
            using (var textReader = File.OpenText(@"Movies.csv"))
            {
                var csv = new CsvReader(textReader);

                // Iterate over each movie, creating it
                foreach (var movie in csv.GetRecords<Movie>())
                {
                    CreateMovie(project, movie);
                }
            }
        }

        private static void CreateMovie(Project project, Movie movie)
        {
            // Set-up a new movie entry
            var movieEntry = project.Entries.New("movie");
            
            // Set each field value
            movieEntry.Set("title", movie.Title);
            movieEntry.Set("overview", movie.Overview);
            movieEntry.Set("runtime", movie.Runtime);
            movieEntry.Set("genres", GetGenreTaxonomyKeys(movie.Genres));
            movieEntry.Set("releaseDate", movie.ReleaseDate);
            movieEntry.Set("revenue", movie.Revenue);

            // Save and publish the movie
            movieEntry.Save();
            movieEntry.Publish();
            Console.WriteLine($"Saved movie {movieEntry.Get("title")}");
        }

        private static string[] GetGenreTaxonomyKeys(string genreNames)
        {
            var genres = genreNames.Split(',');
            return genres.Select(genre => GenreTaxonomyKeys[genre]).ToArray();
        }
    }
}

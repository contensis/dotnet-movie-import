using System;

namespace MovieImport
{
    public class Movie
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public int Runtime { get; set; }
        public string Genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public long Revenue { get; set; }
    }
}

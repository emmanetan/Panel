using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicControlsApp
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Movie()
        {
            Id = -1;
            Title = string.Empty;
            ImagePath = string.Empty;
            ReleaseDate = DateTime.MinValue;
        }

        public Movie(int id, string title, string imagePath, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            ImagePath = imagePath;
            ReleaseDate = releaseDate;
        }

        public void Copy(Movie movie)
        {
            Id = movie.Id;
            Title = movie.Title;
            ImagePath = movie.ImagePath;
            ReleaseDate = movie.ReleaseDate;
        }

    }
}
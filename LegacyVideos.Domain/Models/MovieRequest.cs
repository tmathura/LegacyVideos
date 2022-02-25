using LegacyVideos.Domain.Enums;

namespace LegacyVideos.Domain.Models
{
    public class MovieRequest
    {
        /// <summary>
        /// The title of the movie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the movie.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The movie type.
        /// </summary>
        public MovieType MovieType { get; set; }

        /// <summary>
        /// The duration of the movie.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The release date of the movie.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The date the movie was added.
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Indicator to say if the movie is owned.
        /// </summary>
        public bool Owned { get; set; }
    }
}

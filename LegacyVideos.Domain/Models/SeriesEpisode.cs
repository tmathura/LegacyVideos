using LegacyVideos.Domain.Enums;

namespace LegacyVideos.Domain.Models
{
    public class SeriesEpisode
    {
        /// <summary>
        /// The series episode id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The series id.
        /// </summary>
        public int SeriesId { get; set; }

        /// <summary>
        /// The series episode number.
        /// </summary>
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// The title of the series episode.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the series episode.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The duration of the series episode.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The release date of the series episode.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The date the series episode was added.
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Indicator to say if the series episode is owned.
        /// </summary>
        public bool Owned { get; set; }
    }
}

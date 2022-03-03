namespace LegacyVideos.Domain.Requests
{
    public class AddSeriesRequest
    {
        /// <summary>
        /// The title of the series.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the series.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The release date of the series.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The date the series was added.
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Indicator to say if the series is ended.
        /// </summary>
        public bool Ended { get; set; }

        /// <summary>
        /// Indicator to say if the series is owned.
        /// </summary>
        public bool Owned { get; set; }

        /// <summary>
        /// Indicator to say if the series is an anime.
        /// </summary>
        public bool Anime { get; set; }
    }
}

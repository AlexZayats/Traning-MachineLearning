using Newtonsoft.Json;
using System;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class TVShowSeason : Response
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("episode_count")]
        public int EpisodeCount { get; set; }

        [JsonProperty("season_number")]
        public int SeasonNumber { get; set; }

        [JsonProperty("air_date")]
        public DateTime? AirDate { get; set; }
    }
}

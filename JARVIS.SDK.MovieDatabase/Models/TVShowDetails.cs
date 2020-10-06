using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class TVShowDetails : TVShow
    {
        [JsonProperty("homepage")]
        public string HomePage { get; set; }

        [JsonProperty("number_of_episodes")]
        public int NumberOfEpisodes { get; set; }

        [JsonProperty("number_of_seasons")]
        public int NumberOfSeasons { get; set; }

        [JsonProperty("in_production")]
        public bool InProduction { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("last_air_date")]
        public DateTime LastAirDate { get; set; }

        [JsonProperty("created_by")]
        public IEnumerable<Person> CreatedBy { get; set; }

        [JsonProperty("genres")]
        public IEnumerable<BaseEntity> Genres { get; set; }

        [JsonProperty("production_companies")]
        public IEnumerable<Company> ProductionCompanies { get; set; }

        [JsonProperty("networks")]
        public IEnumerable<Company> Networks { get; set; }

        [JsonProperty("seasons")]
        public IEnumerable<TVShowSeason> Seasons { get; set; }
    }
}

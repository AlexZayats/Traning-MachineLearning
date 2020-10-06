using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class MovieDetails : Movie
    {
        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("homepage")]
        public string HomePage { get; set; }

        [JsonProperty("imdb_id")]
        public string IMDBId { get; set; }

        [JsonProperty("spoken_languages")]
        public IEnumerable<Language> SpokenLanguages { get; set; }

        [JsonProperty("genres")]
        public IEnumerable<BaseEntity> Genres { get; set; }

        [JsonProperty("production_companies")]
        public IEnumerable<Company> ProductionCompanies { get; set; }

        [JsonProperty("production_countries")]
        public IEnumerable<Country> ProductionCountries { get; set; }
    }
}

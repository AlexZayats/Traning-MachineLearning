using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class ConfigurationImages
    {
        [JsonProperty("base_url")]
        public string BaseUrl { get; set; }

        [JsonProperty("secure_base_url")]
        public string SecureBaseUrl { get; set; }

        [JsonProperty("backdrop_sizes")]
        public IEnumerable<string> BackdropSizes { get; set; }

        [JsonProperty("logo_sizes")]
        public IEnumerable<string> LogoSizes { get; set; }

        [JsonProperty("poster_sizes")]
        public IEnumerable<string> PosterSizes { get; set; }

        [JsonProperty("profile_sizes")]
        public IEnumerable<string> ProfileSizes { get; set; }

        [JsonProperty("still_sizes")]
        public IEnumerable<string> StillSizes { get; set; }
    }
}

using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Country
    {
        [JsonProperty("iso_3166_1")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

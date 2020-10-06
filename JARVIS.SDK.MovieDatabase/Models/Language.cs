using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Language
    {
        [JsonProperty("iso_639_1")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

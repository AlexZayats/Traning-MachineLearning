using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Certification
    {
        [JsonProperty("certification")]
        public string Code { get; set; }

        [JsonProperty("meaning")]
        public string Description { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }
}

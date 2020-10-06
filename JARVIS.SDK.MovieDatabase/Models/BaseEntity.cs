using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class BaseEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

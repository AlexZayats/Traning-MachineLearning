using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Response
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
    }
}

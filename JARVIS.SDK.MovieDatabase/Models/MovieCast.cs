using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class MovieCast : Movie
    {
        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("character")]
        public string Caracter { get; set; }
    }
}

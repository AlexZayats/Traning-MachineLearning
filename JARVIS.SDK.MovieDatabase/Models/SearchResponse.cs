using Newtonsoft.Json;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class SearchResponse<T> : Response
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public int Results { get; set; }
    }
}

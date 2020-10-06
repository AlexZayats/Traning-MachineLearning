using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class DiscoverResponse<TModel> : Response
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public IEnumerable<TModel> Results { get; set; }
    }
}
